using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface IExcercisesManager
    {
        Task<ActionResult> AddExcercise(Excercise excercise);
        Task<List<Excercise>> GetExcercises();
        Task<ActionResult> DeleteExcercise(int id);
        Task<ActionResult<Excercise>> GetExcerciseById(int id);
    }
}
