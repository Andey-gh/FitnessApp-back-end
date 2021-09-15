using FitnessWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface ITrainingManager
    {
        void SubmitTraining(User user, EndTrainingViewModel trainingSubmit);
    }
}
