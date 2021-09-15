using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface ITrainingPlanManager
    {
        Task<ActionResult<TrainingPlan>> AddPlan(TrainingPlan plan);
        Task<ActionResult<ICollection<TrainingPlan>>> GetPlanById(int id);
        Task<ActionResult<ICollection<TrainingPlan>>> GetPlansByCategory(string category);
        Task<ActionResult<TrainingPlan>> DeletePlan(int id);
    }
}
