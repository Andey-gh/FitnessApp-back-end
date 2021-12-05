using FitnessWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface IWeightHistoryManager
    {
        Task<bool> AddChange(WeightHistory history);
    }
}
