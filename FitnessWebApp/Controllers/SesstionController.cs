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
    public class SesstionController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public SesstionController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("getPlan/{id}/{day}/{UserId}")]

        public async Task<ActionResult<ICollection<ExcerciseInPlan>>> GetPreSsestion(int id, int day, string userId)
        {
            var plan = await _context.TrainingPlans.FindAsync(id);
            var user = await _userManager.FindByIdAsync(userId);
            var days = new List<int>();


            if (user == null)
                return Unauthorized();

            if (plan != null)
            {

                var userExser = await _context.ExcercisesInPlan.Include(c => c.Excercise).Include(c => c.Excercise.AssistantMuscle).Include(c => c.Excercise.TargetMuscle).Where(p => p.PlanId == id && p.Day == day).Include(c => c.MuscleGroup).ToListAsync();
                if (userExser.Count != 0)
                {
                    List<ExscercisePlanViewModel> excercises = new List<ExscercisePlanViewModel>();
                    for (int i = 0; i < userExser.Count; i++)
                    {
                        ExscercisePlanViewModel excercize = new ExscercisePlanViewModel();
                        excercize.setsNumber = userExser[i].SetsNumber;
                        excercize.Id = userExser[i].Excercise.Id;
                        excercize.Name = userExser[i].Excercise.Name;
                        excercize.TargetMuscleId = userExser[i].Excercise.TargetMuscleId;
                        excercize.AssistantMuscleId = userExser[i].Excercise.AssistantMuscleId;
                        excercize.Description = userExser[i].Excercise.Description;
                        excercize.TargetMuscle = userExser[i].Excercise.TargetMuscle;
                        excercize.AssistantMuscle = userExser[i].Excercise.AssistantMuscle;
                        excercize.Photo = userExser[i].Excercise.Photo;
                        excercises.Add(excercize);
                    }
                    var userExserDays = await _context.ExcercisesInPlan.Where(p => p.PlanId == id).ToListAsync();
                    for (int i = 1; i < 7; i++)
                    {
                        var dayOfWeek = userExserDays.Find(x => x.Day == i);
                        if (dayOfWeek != null)
                        {
                            days.Add(dayOfWeek.Day);
                        }
                    }
                    var trainingPlan = new TrainingPlanViewModel() { planId = userExser[0].PlanId, muscleGroupId = userExser[0].MuscleGroupId, excercises = excercises, muscleGroupName = userExser[0].MuscleGroup.Name, planDiscription = userExser[0].TrainingPlan.Discription, day = userExser[0].Day, activeDays = days };
                    return Json(trainingPlan);
                }
                return NoContent();

            }

            return NoContent();


        }

        [HttpGet("getPreviousTraining/{id}/{MuscleGroupId}/{UserId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PreviousTrainViewModel>> GetPreviousTrainHistory(int id, int muscleGroupId, string userId)
        {
            var plan = await _context.TrainingPlans.FindAsync(id);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized();
            var trHis = await _context.TrainingHistories.Where(x => x.UserId == user.Id && x.PlanId == id && x.MuscleGroupId == muscleGroupId).ToListAsync();
            if (trHis.Count == 0)
            {
                return NoContent();
            }
            var History = trHis.OrderBy(p => p.EndTime).Last();


            List<TrainingHistory> trainingHistory = new List<TrainingHistory>();
            trainingHistory = await _context.TrainingHistories.Where(p => p.UserId == user.Id && p.EndTime.DayOfYear == History.EndTime.DayOfYear && p.MuscleGroupId == muscleGroupId).Include(x => x.Excercise).OrderBy(x => x.Id).ToListAsync();




            PreviousTrainViewModel trainingHistoryView = new PreviousTrainViewModel(id, trainingHistory);


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

        public async Task<ActionResult<ICollection<TrainingHistory>>> GetUserTrainingHistory(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);


            if (user == null) return Unauthorized();

            var trainingHistory = await _context.TrainingHistories.Include(c => c.Excercise).Include(x => x.Excercise.AssistantMuscle).Include(x => x.Excercise.TargetMuscle).Include(x => x.muscleGroup).Where(p => p.UserId == user.Id).Select(x => new { x.Excercise, x.EndTime, x.Quantity, x.muscleGroup, x.TotalWeight, x.StartTime }).ToListAsync(); //.Select(x=>new {x.Excercise,x.EndTime,x.Quantity,x.muscleGroup,x.TotalWeight })
            List<TrainingHistoryViewModel> trainingHistoryViews = new List<TrainingHistoryViewModel>();
            for (int i = 0; i < trainingHistory.Count; i++)
            {
                List<TrainingHistoryExscerciseViewModel> excercises = new List<TrainingHistoryExscerciseViewModel>();
                var Date = trainingHistory[i].EndTime;
                while (trainingHistory[i].EndTime.DayOfYear == Date.DayOfYear)
                {

                    excercises.Add(new TrainingHistoryExscerciseViewModel() { exserciseId = trainingHistory[i].Excercise.Id, exserciseName = trainingHistory[i].Excercise.Name, quantity = trainingHistory[i].Quantity, weight = trainingHistory[i].TotalWeight, startTime = trainingHistory[i].StartTime, endTime = trainingHistory[i].EndTime });
                    i++;
                    if (i >= trainingHistory.Count)
                    {
                        break;
                    }
                }

                trainingHistoryViews.Add(new TrainingHistoryViewModel() { date = Date, excercises = excercises });
                if (i >= trainingHistory.Count)
                {
                    break;
                }
            }

            if (trainingHistoryViews != null)
            {

                return Json(trainingHistoryViews);

            }

            return NoContent();


        }


    }
}
