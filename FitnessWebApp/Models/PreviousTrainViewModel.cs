using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class PreviousTrainViewModel
    {
        public int trainingPlanId { get; set; }
        public List<ExsercisesPreviousModel> exercises { get; set; }
        public PreviousTrainViewModel()
        {

        }
        public PreviousTrainViewModel(int PlanId, List<TrainingHistory> models)
        {
            trainingPlanId = PlanId;
            this.exercises = new List<ExsercisesPreviousModel>();
            
            for (int i=0;i<models.Count;i++)
            {
                ExsercisesPreviousModel exer = new ExsercisesPreviousModel();
                exer.exserciseId = models[i].ExcerciseId;
                exer.exserciseName = models[i].Excercise.Name;
                exer.quantity = models[i].Quantity;
                exer.weight = models[i].TotalWeight;
                this.exercises.Add(exer);
            }

        }
    }
}
