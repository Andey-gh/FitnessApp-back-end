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
    public class PreSesstionController:Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> userManager;

        public PreSesstionController(AppDbContext context, UserManager<User> userMgr)
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
       

    }
}
