using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FitnessWebApp.Controllers
{
    
    [Route("/api")]
    [ApiController]
    public class SesstionController:Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> userManager;

        public SesstionController(AppDbContext context, UserManager<User> userMgr)
        {

            _context = context;
            userManager = userMgr;
        }

        [HttpGet("getPlan/{id}/{day}/{UserId}")]
        
        public async Task<ActionResult<ICollection<ExcerciseInPlan>>> GetPreSsestion(int Id,int Day,string UserId)
        {
            var plan = await _context.TrainingPlans.FindAsync(Id);
            var user = await userManager.FindByIdAsync(UserId);
           
           
                if (user == null)
                    return Unauthorized();

            if (plan!= null) 
            {
                var userplan = await _context.PlansOfUsers.Where(p => p.UserId == user.Id&&p.PlanId==Id).ToListAsync();
                if (userplan.Count!=0)
                {
                    var userExser= await _context.ExcercisesInPlan.Include(c => c.Excercise).Include(c => c.Excercise.AssistantMuscle).Include(c => c.Excercise.TargetMuscle).Where(p => p.PlanId == Id && p.Day == Day).Include(c => c.MuscleGroup).ToListAsync();
                    if(userExser.Count!=0)
                    { 
                    List<ExscercisePlanViewModel> excercises = new List<ExscercisePlanViewModel>();
                    for(int i=0;i<userExser.Count;i++)
                    {
                            ExscercisePlanViewModel excer = new ExscercisePlanViewModel();
                            excer.setsNumber = userExser[i].SetsNumber;
                            excer.Id = userExser[i].Excercise.Id;
                            excer.Name = userExser[i].Excercise.Name;
                            excer.TargetMuscleId = userExser[i].Excercise.TargetMuscleId;
                            excer.AssistantMuscleId= userExser[i].Excercise.AssistantMuscleId;
                            excer.Description = userExser[i].Excercise.Description;
                            excer.TargetMuscle = userExser[i].Excercise.TargetMuscle;
                            excer.AssistantMuscle = userExser[i].Excercise.AssistantMuscle;
                            excercises.Add(excer);
                    }
                    var trainingPlan = new TrainingPlanViewModel() { planId = userExser[0].PlanId, muscleGroupId = userExser[0].MuscleGroupId, excercises = excercises, muscleGroupName = userExser[0].MuscleGroup.Name, planDiscription = userExser[0].TrainingPlan.Discription,day=userExser[0].Day};
                        return Json(trainingPlan);
                    }
                    return Forbid();
                }
                else
                    return Forbid();
            }
            
            return NoContent();

            
        }

        [HttpGet("getPreviousTraining/{id}/{MuscleGroupId}/{UserId}")]

        public async Task<ActionResult<PreviousTrainViewModel>> GetPreviousTrainHistory(int Id,int MuscleGroupId,string UserId)
        {
            var plan = await _context.TrainingPlans.FindAsync(Id);
           
            var user = await userManager.FindByIdAsync(UserId);

                if (user == null)
                    return Unauthorized();
            var user_plans = await _context.PlansOfUsers.Where(x => x.UserId == user.Id).ToListAsync();
            if (user_plans.Count==0)
                return NoContent();

            var trHis = await _context.TrainingHistories.Where(x => x.UserId == user.Id && x.PlanId == Id&&x.MuscleGroupId==MuscleGroupId).ToListAsync();
            if (trHis.Count == 0)
            {
                return NoContent();
            }
            var History = trHis.OrderBy(p => p.EndTime).Last();
           
           
                List<TrainingHistory> trainingHistory = new List<TrainingHistory>();
                trainingHistory = await _context.TrainingHistories.Where(p =>p.UserId==user.Id&&p.EndTime.DayOfYear==History.EndTime.DayOfYear&&p.MuscleGroupId==MuscleGroupId).Include(x=>x.Excercise).ToListAsync();
          
            
                
                
            PreviousTrainViewModel trainingHistoryView = new PreviousTrainViewModel(Id, trainingHistory); 


            if (plan != null)
            {
                if (trainingHistory.Count != 0)
                {


                    return Json(trainingHistoryView);
                }
                else
                    return Forbid();
            }

            return NoContent();


        }
        [HttpGet("TrainingHistory/{UserId}")]

        public async Task<ActionResult<ICollection<TrainingHistory>>> GetUserTrainingHistory(string UserId)
        {
            
            var user = await userManager.FindByIdAsync(UserId);
            
            {
                if (user == null)
                    return Unauthorized();
            }
            var trainingHistory = await _context.TrainingHistories.Include(c=>c.Excercise).Include(x=>x.muscleGroup).Where(p => p.UserId == user.Id ).Select(x=>new {x.Excercise,x.EndTime,x.Quantity,x.muscleGroup,x.TotalWeight }).ToListAsync();


            if (trainingHistory != null)
            {

                    return Json(trainingHistory);
                
            }

            return NoContent();


        }


    }
}
