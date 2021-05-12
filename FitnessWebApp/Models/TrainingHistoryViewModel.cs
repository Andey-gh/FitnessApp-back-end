using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingHistoryViewModel
    {
        public DateTime date { get; set; }

        public List<TrainingHistoryExscerciseViewModel> excercises { get; set; }
    }
}
