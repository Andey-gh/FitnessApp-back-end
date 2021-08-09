using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingHistoryExscerciseViewModel
    {
        public int exserciseId { get; set; }
        public string  exserciseName { get; set; }
        public float weight { get; set; }
        public int quantity { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
