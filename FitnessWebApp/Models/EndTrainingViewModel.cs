using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class EndTrainingViewModel
    {
        public int trainingPlanId { get; set; }
        public int muscleGroupId { get; set; }
        public List<ExcercisesEndTrainingModel> exercises { get; set; }
        public EndTrainingViewModel()
        {

        }

        public EndTrainingViewModel(int Planid,List<TrainingHistory> trainingHistories)
        {
            trainingPlanId = Planid;
            muscleGroupId = trainingHistories[0].MuscleGroupId;

            exercises = new List<ExcercisesEndTrainingModel>();
            for(int i=0;i<trainingHistories.Count;i++)
            {
                ExcercisesEndTrainingModel excercisesEndTrainingModel=new ExcercisesEndTrainingModel();
                
                excercisesEndTrainingModel.exerciseId = trainingHistories[i].ExcerciseId;
                excercisesEndTrainingModel.kg = trainingHistories[i].TotalWeight;
                excercisesEndTrainingModel.quantity = trainingHistories[i].Quantity;
                excercisesEndTrainingModel.startTime = trainingHistories[i].StartTime;
                excercisesEndTrainingModel.endTime = trainingHistories[i].EndTime;
                exercises.Add(excercisesEndTrainingModel);
          
            }

        }


    }
    
}
