using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using FitnessWebApp.Models;
using FitnessWebApp.Services;
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
    [ApiController]
    public class StatisticsController : Controller
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;
        private readonly IStatisticsManager _statisticsManager;
        private readonly JWTservice _jwtService;
        public StatisticsController(AppDbContext context, UserManager<User> userManager, IStatisticsManager statisticsManager, JWTservice jwtService)
        {
            _context = context;
            _userManager = userManager;
            _statisticsManager = statisticsManager;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("Tonnage")]
        public async Task<IActionResult> GetTonnage(Statistics stats)
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

            return await _statisticsManager.GetTonnage(stats);

        }

        [HttpPost]
        [Route("GetWeight")]
        public async Task<IActionResult> WeightChange(WeightHistory history)
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

            return await _statisticsManager.WeightChange(history);
        }

    }
}
