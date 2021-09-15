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
using Microsoft.Extensions.Configuration;
using FitnessWebApp.Services;

namespace FitnessWebApp.Controllers
{

    [Route("/api")]
    [ApiController]
    public class TrainingPlanController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITrainingPlanManager _trainingPlanManager;
        private readonly JWTservice _jwtService;
        public TrainingPlanController(AppDbContext context, UserManager<User> userManager, IConfiguration configuration, JWTservice jwtService, TrainingPlanManager trainingPlanManager)
        {

            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
            _trainingPlanManager = trainingPlanManager;
        }

        [HttpPost]
        [Route("TrainingPlans")]
        public async Task<ActionResult<TrainingPlan>> Post([FromForm] TrainingPlan plan)
        {
            if (!ModelState.IsValid) 
            {
                return UnprocessableEntity();
            }
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
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

            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
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
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
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
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }

            return await _trainingPlanManager.DeletePlan(id);
 
        }
    }
}
