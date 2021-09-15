using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface IStatisticsManager
    {
        public Task<IActionResult> GetTonnage(Statistics stats);
        public Task<IActionResult> WeightChange(WeightHistory history);
    }
}
