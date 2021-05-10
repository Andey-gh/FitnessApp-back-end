using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FitnessWebApp.Models;
using FitnessWebApp.Domain;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FitnessWebApp.Controllers
{
    [Authorize]
    [Route("/api")]
    [ApiController]
    public class TrainingPlanController:Controller
    {
        //private  TrainingPlanManager _trainingPlanManager;
        private  AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public TrainingPlanController(AppDbContext context, UserManager<User> userManager)
        {
         
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("TrainingPlans")]
        public async Task<ActionResult<TrainingPlan>> Post(TrainingPlan plan)
        {
            if (ModelState.IsValid) {
                await _context.AddAsync(plan);
            await _context.SaveChangesAsync();
            return Json(plan); 
            }
            return UnprocessableEntity();
               
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetPlanById/{id}/{UserId}")]
        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlans(int id,string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if(user==null)
            {
                return Unauthorized();
            }
            var plan = _context.TrainingPlans.Where(x => x.Id == id).FirstOrDefault();
            if(plan==null)
            {
                return NoContent();
            }
            //var exscercises =await  _context.ExcercisesInPlan.Where(x => x.PlanId == id).Include(x => x.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).ToListAsync();
            List<List<ExscercisePlanViewModel>> trainings = new List<List<ExscercisePlanViewModel>>();
            for(int i=1;i<8;i++)
            {
                List<ExscercisePlanViewModel> exscercisePlanViews = new List<ExscercisePlanViewModel>();
                var exscer = await _context.ExcercisesInPlan.Where(x => x.PlanId == id&&x.Day==i).Include(x => x.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).ToListAsync();

                foreach (var a in exscer)
                {
                    ExscercisePlanViewModel exscercisePlanViewModel = new ExscercisePlanViewModel() { Id = a.Id, Name = a.Excercise.Name, Description = a.Excercise.Description, setsNumber = a.SetsNumber, TargetMuscle = a.Excercise.TargetMuscle, TargetMuscleId = a.Excercise.TargetMuscleId, AssistantMuscle = a.Excercise.AssistantMuscle, AssistantMuscleId = a.Excercise.AssistantMuscleId };
                    exscercisePlanViews.Add(exscercisePlanViewModel);
                }

                trainings.Add(exscercisePlanViews);
            }
            TrainingPlanByCategoryViewModel trainingPlanByCategory = new TrainingPlanByCategoryViewModel() { planId=plan.Id,category=plan.Category,trainings=trainings,planDescription=plan.Discription,planName=plan.Name,photo=plan.Photo};
            return Json(trainingPlanByCategory);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("TrainingPlansByCategory/{category}")]
        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlansByCategory(string category)
        {
            //List<TrainingPlanByCategoryViewModel> trainingPlanByCategory=new List<TrainingPlanByCategoryViewModel>();
           // List<ExscercisePlanViewModel> excerciseInPlan;
            var plans=await _context.TrainingPlans.Where(x => x.Category == category).ToListAsync();
            if(plans.Count==0)
            {
                return NoContent();
            }
            /*for(int i=0;i<plans.Count;i++)
            {
                //excerciseInPlan = new List<ExscercisePlanViewModel>();
               // var exscercises = await _context.ExcercisesInPlan.Include(x=>x.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).Where(x => x.PlanId == plans[i].Id).ToListAsync();
                /*if(exscercises.Count==0)
                {
                    return NoContent();
                }*/
                /*foreach(var a in exscercises)
                {
                    ExscercisePlanViewModel exscercisePlanViewModel = new ExscercisePlanViewModel() { Id = a.Id, Name = a.Excercise.Name, Description = a.Excercise.Description, setsNumber = a.SetsNumber, TargetMuscle = a.Excercise.TargetMuscle, TargetMuscleId = a.Excercise.TargetMuscleId, AssistantMuscle = a.Excercise.AssistantMuscle, AssistantMuscleId = a.Excercise.AssistantMuscleId };
                    excerciseInPlan.Add(exscercisePlanViewModel);
                }*/
                
                
                /*TrainingPlanByCategoryViewModel PlanByCategory = new TrainingPlanByCategoryViewModel() {planId=plans[i].Id,category=plans[i].Category};
                trainingPlanByCategory.Add(PlanByCategory);
            }*/
            
            return Json(plans);
        }
        [HttpGet("TrainingPlans/{id}")]

        public async Task<ActionResult<TrainingPlan>> GetPlan(int id)
        {
            if (ModelState.IsValid) 
            {
                var plan_id = await _context.TrainingPlans.FindAsync(id);

            if (plan_id == null)
            {
                return NotFound();
            }

            return Json(plan_id); 

            }
            return UnprocessableEntity();
                
        }

        [HttpDelete("TrainingPlans/{id}")]
        public async Task<ActionResult<TrainingPlan>> DeletePlan(int id)
        {
            if (ModelState.IsValid) { 
                var plan = await _context.TrainingPlans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            _context.TrainingPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return Ok("Plan was deleted"); 
            }
            return UnprocessableEntity();
               
        }
    }
}
