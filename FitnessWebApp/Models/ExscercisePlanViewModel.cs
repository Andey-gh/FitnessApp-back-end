using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class ExscercisePlanViewModel
    {
  
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public int? TargetMuscleId { get; set; }
       
        public int? AssistantMuscleId { get; set; }
        public int setsNumber { get; set; }
        public virtual Muscle TargetMuscle { get; set; }
        public virtual Muscle AssistantMuscle { get; set; }


    }
}
