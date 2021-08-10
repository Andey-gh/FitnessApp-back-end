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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FitnessWebApp.Managers;

namespace FitnessWebApp.Controllers
{
    
    [Route("/api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TrainingPlanController:Controller
    {
        //private  TrainingPlanManager _trainingPlanManager;
        private  readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly TrainingPlanManager _trainingPlanManager;
        public TrainingPlanController(AppDbContext context, UserManager<User> userManager)
        {
            _trainingPlanManager = new TrainingPlanManager(context);
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("TrainingPlans")]
        public async Task<ActionResult<TrainingPlan>> Post(TrainingPlan plan)
        {
            if (!ModelState.IsValid) 
            {
                return UnprocessableEntity();
            }
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

            return await _trainingPlanManager.AddPlan(plan); 

        }

        [HttpGet]
        [Route("GetPlanById/{id}")]
        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlans(int id)
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
            return await _trainingPlanManager.GetPlanById(id);
        }


        [HttpGet]
        [Route("TrainingPlansByCategory/{category}")]
        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlansByCategory(string category)
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
            
            return await _trainingPlanManager.GetPlansByCategory(category);
        }

       //[HttpGet("TrainingPlans/{id}")]
       //
       //public async Task<ActionResult<TrainingPlan>> GetPlan(int id)
       //{
       //    if (ModelState.IsValid) 
       //    {
       //        var plan_id = await _context.TrainingPlans.FindAsync(id);
       //
       //    if (plan_id == null)
       //    {
       //        return NotFound();
       //    }
       //
       //    return Json(plan_id); 
       //
       //    }
       //    return UnprocessableEntity();
       //        
       //}

        [HttpDelete("TrainingPlans/{id}")]
        public async Task<ActionResult<TrainingPlan>> DeletePlan(int id)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
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

            return await _trainingPlanManager.DeletePlan(id);
 
        }
    }
}
