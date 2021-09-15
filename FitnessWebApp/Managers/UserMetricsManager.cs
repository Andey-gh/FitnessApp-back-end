using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class UserMetricsManager:IUserMetricsManager
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserMetricsManager(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            userManager = _userManager;
        }

        public  UserProfileViewModel GetUserMetrics(User user)
        {
            var user_health = _context.HealthProblems.Where(x => x.UserId == user.Id).ToList();
            List<string> health_problems = new List<string>();
            if (user_health != null)
            {

                for (int i = 0; i < user_health.Count; i++)
                {
                    health_problems.Add(user_health[i].Problem);
                }
            }
            var user_metrics = new UserProfileViewModel() { MetricAge = user.Age, MetricGoal = user.Goal, HealthProblems = health_problems, MetricHeight = user.Height, MetricPullUps = user.MaxPullUps, MetricPushUps = user.MaxPushUps, MetricWeight = user.Weight, Name = user.Name, MetricGender = user.Gender };

            return user_metrics;
        }
        public async void UpdateUserMetrics (User user, UserMetricsUpdateModel UserMetrics)
        {
            user.Name = UserMetrics.Name;
            WeightHistory history = new WeightHistory(user.Id, UserMetrics.MetricWeight, DateTime.Now.Date);
            WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
            await manager.AddChange(history);
            user.Age = UserMetrics.MetricAge;
            user.Gender = UserMetrics.MetricGender;
            user.Goal = UserMetrics.MetricGoal;
            user.Height = UserMetrics.MetricHeight;
            for (int i = 0; i < UserMetrics.healthProblems.Count; i++)
            {
                UserMetrics.healthProblems[i].UserId = user.Id;
            }
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            var user_health = _context.HealthProblems.Where(x => x.UserId == user.Id).ToList();
            _context.HealthProblems.RemoveRange(user_health);
            await _context.SaveChangesAsync();
            await _context.HealthProblems.AddRangeAsync(UserMetrics.healthProblems);
            await _context.SaveChangesAsync();
            
        }
        public async void PostUserMetrics(User user, MetricsModel model)
        {
            user.Age = model.MetricAge;
            user.Height = model.MetricHeight;
            WeightHistory history = new WeightHistory(user.Id, model.MetricWeight, DateTime.Now.Date);
            WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
            await manager.AddChange(history);
            user.Goal = model.MetricGoal;
            user.MaxPushUps = model.MetricPushUps;
            user.MaxPullUps = model.MetricPullUps;
            user.IsMetrics = true;


            for (int i = 0; i < model.MetricHealth.Count; i++)
            {
                HealthProblem problem = new HealthProblem(user.Id, model.MetricHealth[i].Problem);

                await _context.HealthProblems.AddAsync(problem);
                await _context.SaveChangesAsync();
            }


            await _userManager.UpdateAsync(user);
        }
        public async Task<IActionResult> ChangeActivePlan(int planId,User user)
        {
            var plan = _context.TrainingPlans.Where(x => x.Id == planId).FirstOrDefault();
            if (plan == null)
            {
                return new NoContentResult();
            }
            user.ActivePlanId = plan.Id;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
