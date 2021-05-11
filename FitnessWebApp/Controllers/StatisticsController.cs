using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using FitnessWebApp.Models;
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
        public StatisticsController(AppDbContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Tonnage")]
        public async Task<IActionResult> GetTonnage(Statistics stats)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            string[] labels = new string[stats.Period];
            float[] tonnages = new float[stats.Period];
            DateTime now = DateTime.Now;
            for (int i = stats.Period-1; i >= 0; i--)labels[stats.Period-i-1] =(now.AddDays(-i)).Date.ToString("dd.MM");

            if (stats.Step == "Day")
            {
                var selectedExcercises = _context.TrainingHistories.Where(ex => (ex.UserId == stats.UserId)).ToList();
                foreach (TrainingHistory ex in selectedExcercises)
                {
                    TimeSpan difference = now.Date - ex.StartTime.Date;
                    if (difference.Days < stats.Period){
                        int pos = stats.Period - difference.Days-1;
                        tonnages[pos] += ex.TotalWeight * ex.Quantity;
                    }
                }
            }
            else {
                return StatusCode(400);
            }
            object[] objs = new object[stats.Period];
            for (int i = 0; i < stats.Period; i++) objs[i] = new { X = i ,Label = labels[i], Value = tonnages[i] };
            return Json(objs);

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Weight")]
        public async Task<IActionResult> WeightChange(WeightHistory history){
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            WeightHistoryManager manager  = new WeightHistoryManager(_context,_userManager);
            bool result = await manager.AddChange(history);
            if (!result) return StatusCode(400);
            return Ok();
        }




    }
}
