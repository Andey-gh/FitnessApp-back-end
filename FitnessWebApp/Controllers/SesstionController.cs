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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FitnessWebApp.Managers;
using FitnessWebApp.Services;

namespace FitnessWebApp.Controllers
{
    
    [Route("/api")]
    [ApiController]
    public class SesstionController:Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ISessionManager _sessionManager;
        private readonly JWTservice _jwtService;
        public SesstionController(AppDbContext context, UserManager<User> userMgr, ISessionManager sessionManager, JWTservice jwtService)
        {

            _context = context;
            _userManager = userMgr;
            _sessionManager = sessionManager;
            _jwtService = jwtService;
        }

        [HttpGet("getPlan/{id}/{day}")]
        
        public async Task<ActionResult<ICollection<ExcerciseInPlan>>> GetPreSsestion(int Id,int Day)
        {
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }
            var plan = await _context.TrainingPlans.FindAsync(Id);
            
            if (plan!= null) 
            {
               var userExser= await _context.ExcercisesInPlan.Include(c => c.Excercise).Include(c => c.Excercise.AssistantMuscle).Include(c => c.Excercise.TargetMuscle).Where(p => p.PlanId == Id && p.Day == Day).Include(c => c.MuscleGroup).ToListAsync();
               if(userExser.Count!=0)
               { 
                   return Json(_sessionManager.GetPlanByIdAndDay(Id,Day,userExser));
               }
                return NoContent();
            }
            
            return NoContent();

        }

        [HttpGet("getPreviousTraining/{id}/{MuscleGroupId}")]

        public async Task<ActionResult<PreviousTrainViewModel>> GetPreviousTrainHistory(int Id,int MuscleGroupId)
        {
            var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            if (UserId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            return  await _sessionManager.GetPreviousTrainingByIdAndMuscleGroup(Id, MuscleGroupId, user);

        }
        [HttpGet("TrainingHistory")]

        public async Task<ActionResult<ICollection<TrainingHistory>>> GetUserTrainingHistory()
        {
            var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            if (UserId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            return await _sessionManager.GetTrainingHistory(user);

        }

    }
}
