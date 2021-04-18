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
    [Authorize]
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

        [HttpGet("getPlan/{id}/{day}")]
        
        public async Task<ActionResult<ICollection<ExcerciseInPlan>>> GetPreSsestion(int Id,int Day)
        {
            var plan = await _context.TrainingPlans.FindAsync(Id);
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userplan = await _context.PlansOfUsers.Where(p => p.UserId == user.Id&&p.PlanId==Id).ToListAsync();
            
            
            if (plan!= null)
            {
                if (userplan.Count!=0)
                {


                    return await _context.ExcercisesInPlan.Include(c => c.Excercise).Where(p => p.PlanId == Id && p.Day == Day).Include(c => c.MuscleGroup).ToListAsync();
                }
                else
                    return Forbid();
            }
            
            return NoContent();

            
        }

        [HttpGet("getPreviousTraining/{id}")]

        public async Task<ActionResult<PreviousTrainViewModel>> GetPreviousTrainHistory(int Id)
        {
            var plan = await _context.TrainingPlans.FindAsync(Id);
            List<ExcerciseInPlan> exsercise=new List<ExcerciseInPlan>();
            List<TrainingHistory> trainingHistories = new List<TrainingHistory>();
            //List<MuscleGroup> muscleGroups = new List<MuscleGroup>();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var user_plans = await _context.PlansOfUsers.Where(x => x.UserId == user.Id).ToListAsync();
            var trHis = await _context.TrainingHistories.OrderBy(x => x.UserId == user.Id && x.PlanId == Id).OrderBy(p=>p.EndTime).LastAsync();
            for(int i=0;i<user_plans.Count;i++)
            {
                List<ExcerciseInPlan> excer = new List<ExcerciseInPlan>();
               excer = await _context.ExcercisesInPlan.Where(x=>x.PlanId==Id&&x.PlanId==user_plans[i].PlanId).ToListAsync();
                for(int k=0;k<excer.Count;k++)
                {
                    if (k == 0) { exsercise.Add(excer[k]);}
                    if (k != 0 && excer[k].Day == excer[k - 1].Day) { exsercise.Add(excer[k]); }
                
                
                }
            }

            for (int i = 0; i < exsercise.Count; i++) 
            {
                List<TrainingHistory> trainingHistory = new List<TrainingHistory>();
                trainingHistory = await _context.TrainingHistories.Where(p =>p.ExcerciseId==exsercise[i].ExcerciseId&&p.UserId==user.Id&&p.EndTime.DayOfYear==trHis.EndTime.DayOfYear).Include(x=>x.Excercise).ToListAsync();
                trainingHistories.AddRange(trainingHistory);
            }
            
                
                
            PreviousTrainViewModel trainingHistoryView = new PreviousTrainViewModel(Id, trainingHistories); 


            if (plan != null)
            {
                if (trainingHistories.Count != 0)
                {


                    return Json(trainingHistoryView);
                }
                else
                    return Forbid();
            }

            return NoContent();


        }


    }
}
