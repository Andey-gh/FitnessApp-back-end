using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class StatisticsController : Controller
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;
        private readonly StatisticsManager _statisticsManager;
        public StatisticsController(AppDbContext context, UserManager<User> userManager) 
        {
            _context = context;
            _userManager = userManager;
            _statisticsManager = new StatisticsManager(context,userManager);
        }

        [HttpGet]
        [Route("Tonnage")]
        public async Task<IActionResult> GetTonnage(Statistics stats)
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
            
            return await _statisticsManager.GetTonnage(stats);

        }

        [HttpPost]
        [Route("Weight")]
        public async Task<IActionResult> WeightChange(WeightHistory history)
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
            
            return await _statisticsManager.WeightChange(history);
        }

    }
}
