using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class ExcerciseInPlan
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [Display(Name = "Id Плана тренировок")]
        
        [ForeignKey(nameof(TrainingPlan))]
        public int? PlanId { get; set; }
        
        public TrainingPlan TrainingPlan { get; set; } 
        
        [Required]
        [Display(Name = "Id Упражнения")]
        [ForeignKey(nameof(Excercise))]
        public int? ExcerciseId { get; set; }

        public Excercise Excercise { get; set; } 
        
        [Required]
        [Display(Name = "День упражнения")]
        public int Day { get; set; }

        [Required]
        [Display(Name = "Id группы мышц")]
        [ForeignKey(nameof(MuscleGroup))]
        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        
        [Required]
        [Display(Name = "Количество подходов в упражнении")]
        public int SetsNumber { get; set; }

        
    }
}
