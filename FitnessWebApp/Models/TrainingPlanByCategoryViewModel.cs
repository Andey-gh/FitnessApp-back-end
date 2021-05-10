using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingPlanByCategoryViewModel
    {
        public int planId { get; set; }
        public string planName { get; set; }
        public string planDescription { get; set; }
        public string category { get; set; }
        public string photo { get; set; }
        public List<List<ExscercisePlanViewModel>> trainings { get; set; }
    }
}
