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
        public DateTime StartTime { get; set; }
        
        [Display(Name = "Время окончания")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Общий вес")]
        public float TotalWeight { get; set; }

        public int Quantity { get; set; }
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
        public int MuscleGroupId { get; set; }
        [ForeignKey(nameof(MuscleGroupId))]
        public MuscleGroup muscleGroup { get; set; }
        public TrainingHistory( int PlanId, int ExcerciseId, float Kg, int Quantity,DateTime StartTime,DateTime EndTime ,string UserId,int muscleGroupId )
        {
            this.TotalWeight = Kg;
            this.PlanId = PlanId;
            this.ExcerciseId = ExcerciseId;
            this.Quantity = Quantity;
            this.UserId = UserId;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.MuscleGroupId = muscleGroupId;
            
        }
        public TrainingHistory()
        {

        }
    }
}
