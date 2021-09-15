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
using Microsoft.Extensions.Configuration;
using FitnessWebApp.Services;

namespace FitnessWebApp.Controllers
{
    
    [Route("/api")]
    [ApiController]
    public class ExcerciseController : Controller
    {
        
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IExcercisesManager _excerciseManager;
        private readonly JWTservice _jwtService;


        public ExcerciseController(AppDbContext context, UserManager<User> userManager, IConfiguration configuration, IExcercisesManager excerciseManager, JWTservice jwtService)
        {
            _userManager = userManager;
            _context = context;
            _excerciseManager = excerciseManager;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("Excercises")]
        public async Task<ActionResult> Post([FromForm]Excercise excercise)
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

            return await _excerciseManager.AddExcercise(excercise);

        }

        [HttpGet]
        [Route("Excercises")]
        public async Task<ActionResult<ICollection<Excercise>>> GetExcercises()
        {
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
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
                var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
                if (user == null)
                {
                    return Unauthorized();
                }
                return await _excerciseManager.GetExcerciseById(id);

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
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _excerciseManager.DeleteExcercise(id);
  
        }
    }
}
