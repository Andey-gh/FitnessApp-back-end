using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface ITrainingManager
    {
        Task<ActionResult> SubmitTraining(User user, EndTrainingViewModel trainingSubmit);
    }
}
