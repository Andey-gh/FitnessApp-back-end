using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingPlanByCategoryViewModel
    {
        public int planId { get; set; }
        public string category { get; set; }
        public List<ExscercisePlanViewModel> exscercises { get; set; }
    }
}
