using FitnessWebApp.Models;
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
