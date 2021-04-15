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

namespace FitnessWebApp.Controllers
{
    [Authorize]
    [Route("/api")]
    [ApiController]
    public class TrainingPlanController:Controller
    {
        //private  TrainingPlanManager _trainingPlanManager;
        private  AppDbContext _context;

        public TrainingPlanController(AppDbContext context)
        {
         
            _context = context;
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
        [Route("TrainingPlans")]
        public async Task<ActionResult<ICollection<TrainingPlan>>> GetPlans()
        {
               
            return await _context.TrainingPlans.ToListAsync();
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
