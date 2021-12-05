using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class TrainingPlanManager:ITrainingPlanManager
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public TrainingPlanManager(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<TrainingPlan>> AddPlan(TrainingPlan plan)
        {
            string extension = Path.GetExtension(plan.Image.FileName);
            if (extension.ToLower() != ".jpg" && extension.ToLower() != ".png") return new StatusCodeResult(400);
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", "Images", guid + ".jpg");
            if (plan.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                plan.Image.CopyTo(fileStream);
            }
            plan.Photo = $"{ _configuration.GetValue<string>("Domain")}" + filePath.Remove(0, 7);
            await _context.AddAsync(plan);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlanById(int id)
        {
            var plan = await _context.TrainingPlans.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (plan == null)
            {
                return new NoContentResult();
            }
            //var exscercises =await  _context.ExcercisesInPlan.Where(x => x.PlanId == id).Include(x => x.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).ToListAsync();
            List<List<ExscercisePlanViewModel>> trainings = new List<List<ExscercisePlanViewModel>>();
            for (int i = 1; i < 8; i++)
            {
                List<ExscercisePlanViewModel> exscercisePlanViews = new List<ExscercisePlanViewModel>();
                var exscer = await _context.ExcercisesInPlan.Where(x => x.PlanId == id && x.Day == i).Include(x => x.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).ToListAsync();

                foreach (var a in exscer)
                {
                    ExscercisePlanViewModel exscercisePlanViewModel = new ExscercisePlanViewModel() { Id = a.Id, Name = a.Excercise.Name, Description = a.Excercise.Description, setsNumber = a.SetsNumber, TargetMuscle = a.Excercise.TargetMuscle, TargetMuscleId = a.Excercise.TargetMuscleId, AssistantMuscle = a.Excercise.AssistantMuscle, AssistantMuscleId = a.Excercise.AssistantMuscleId };
                    exscercisePlanViews.Add(exscercisePlanViewModel);
                }

                trainings.Add(exscercisePlanViews);
            }
            TrainingPlanByCategoryViewModel trainingPlanByCategory = new TrainingPlanByCategoryViewModel() { planId = plan.Id, category = plan.Category, trainings = trainings, planDescription = plan.Discription, planName = plan.Name, photo = plan.Photo };
            return new JsonResult(trainingPlanByCategory);
        }

        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlansByCategory(string category)
        {
            //List<TrainingPlanByCategoryViewModel> trainingPlanByCategory=new List<TrainingPlanByCategoryViewModel>();
            // List<ExscercisePlanViewModel> excerciseInPlan;
            var plans = await _context.TrainingPlans.Where(x => x.Category == category).ToListAsync();
            if (plans.Count == 0)
            {
                return new NoContentResult();
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

            return new JsonResult(plans);
        }

        public async Task<ActionResult<TrainingPlan>> DeletePlan(int id)
        {
            var plan = await _context.TrainingPlans.FindAsync(id);
            if (plan == null)
            {
                return new NotFoundResult();
            }

            _context.TrainingPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult<ICollection<TrainingPlan>>> GetAllPlans()
        {
            var plan = await _context.TrainingPlans.ToListAsync();
            if (plan == null)
            {
                return new NotFoundResult();
            }
            return new JsonResult(plan);
        }
    }
}
