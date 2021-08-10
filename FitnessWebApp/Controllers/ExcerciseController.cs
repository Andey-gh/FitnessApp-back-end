using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using FitnessWebApp.Managers;

namespace FitnessWebApp.Controllers
{
    
    [Route("/api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ExcerciseController : Controller
    {
        
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ExcercisesManager _excerciseManager;

        public ExcerciseController(AppDbContext context,UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
            _excerciseManager = new ExcercisesManager(context);
        }

        [HttpPost]
        [Route("Excercises")]
        public async Task<ActionResult> Post(Excercise excercise)
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
            _excerciseManager.AddExcercise(excercise);
            return Ok();

        }

        [HttpGet]
        [Route("Excercises")]
        public async Task<ActionResult<ICollection<Excercise>>> GetExcercises()
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

            return await _excerciseManager.GetExcercises();
        }


        [HttpGet("Excercises/{id}")]

        public async Task<ActionResult<Excercise>> GetExcercise(int id)
        {
            if (ModelState.IsValid)
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
                var excercise_id = await _context.Excercises.FindAsync(id);

                if (excercise_id == null)
                {
                    return NotFound();
                }

                return Json(excercise_id);

            }
            return UnprocessableEntity();

        }

        [HttpDelete("Excercises/{id}")]
        public async Task<ActionResult<Excercise>> DeleteExcercise(int id)
        {
            if (ModelState.IsValid)
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
            var excercise = await _context.Excercises.FindAsync(id);
            if (excercise == null)
            {
                return NotFound();
            }

            _excerciseManager.DeleteExcercise(excercise);

                return Ok("Excercise was deleted");
  
        }
    }
}
