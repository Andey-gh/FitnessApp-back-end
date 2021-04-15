using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingHistory
    {
        [Key]
        public int Id { get; set; }

       
        [Display(Name = "Время начала")]
        public DataType StartTime { get; set; }
        
        [Display(Name = "Время окончания")]
        public DataType EndTime { get; set; }

        [Required]
        [Display(Name = "Общий вес")]
        public float TotalWeight { get; set; }
        [Required]
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        [Display(Name = "Id плана тренировок")]
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public TrainingPlan TrainingPlan { get; set; }
        [Required]
        [Display(Name = "Id Упражнения")]
        public int ExcerciseId { get; set; }
        [ForeignKey(nameof(ExcerciseId))]
        public Excercise Excercise { get; set; }
    }
}
