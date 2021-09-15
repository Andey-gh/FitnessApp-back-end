using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class TrainingManager:ITrainingManager
    {
        private readonly AppDbContext _context;
        public TrainingManager(AppDbContext context)
        {
            _context = context;
        }

        public async void SubmitTraining(User user,EndTrainingViewModel trainingSubmit)
        {
            for (int i = 0; i < trainingSubmit.exercises.Count; i++)
            {
                TrainingHistory trainingHistory = new TrainingHistory(trainingSubmit.trainingPlanId, trainingSubmit.exercises[i].exerciseId, trainingSubmit.exercises[i].kg, trainingSubmit.exercises[i].quantity, trainingSubmit.exercises[i].startTime, trainingSubmit.exercises[i].endTime, user.Id, trainingSubmit.muscleGroupId);
                await _context.AddAsync(trainingHistory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
