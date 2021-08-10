using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class StatisticsManager
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public StatisticsManager(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> GetTonnage(Statistics stats)
        {
            string[] labels = new string[stats.Period];
            float[] tonnages = new float[stats.Period];
            DateTime now = DateTime.Now;
            for (int i = stats.Period - 1; i >= 0; i--) labels[stats.Period - i - 1] = (now.AddDays(-i)).Date.ToString("dd.MM");

            if (stats.Step == "Day")
            {
                var selectedExcercises = await _context.TrainingHistories.Where(ex => (ex.UserId == stats.UserId)).ToListAsync();
                foreach (TrainingHistory ex in selectedExcercises)
                {
                    TimeSpan difference = now.Date - ex.StartTime.Date;
                    if (difference.Days < stats.Period)
                    {
                        int pos = stats.Period - difference.Days - 1;
                        tonnages[pos] += ex.TotalWeight * ex.Quantity;
                    }
                }
            }
            else
            {
                return new StatusCodeResult(400);
            }
            object[] objs = new object[stats.Period];
            for (int i = 0; i < stats.Period; i++) objs[i] = new { X = i, Label = labels[i], Value = tonnages[i] };
            return new JsonResult(objs);
        }

        public async Task<IActionResult> WeightChange(WeightHistory history)
        {
            history.Date = DateTime.Now.Date;
            WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
            bool result = await manager.AddChange(history);
            if (!result) return new StatusCodeResult(400);
            return new OkResult();
        }
    }
}
