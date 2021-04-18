using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class ExcercisesEndTrainingModel
    {
        
        public int exerciseId { get; set; }
        public float kg { get; set; }
        public int quantity { get; set; }

        
        
        public DateTime startTime { get; set; }

        
        public DateTime endTime { get; set; }

    }
}
