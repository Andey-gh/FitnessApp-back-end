using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingPlanViewModel
    {   public int muscleGroupId { get; set; }
        public string muscleGroupName { get; set; }
        public int day { get; set; }
        public List<int> activeDays { get; set; }
        public int? planId { get; set; }
        public string planDiscription { get; set; }
        public List<ExscercisePlanViewModel> excercises { get; set; }
       
    }
}
