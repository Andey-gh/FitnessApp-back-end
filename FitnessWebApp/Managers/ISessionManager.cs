using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface ISessionManager
    {
        Task<TrainingPlanViewModel> GetPlanByIdAndDay(int Id, int Day, List<ExcerciseInPlan> userExser);
        Task<ActionResult<PreviousTrainViewModel>> GetPreviousTrainingByIdAndMuscleGroup(int Id, int MuscleGroupId, User user);
        Task<ActionResult<ICollection<TrainingHistory>>> GetTrainingHistory(User user);
    }          
}
