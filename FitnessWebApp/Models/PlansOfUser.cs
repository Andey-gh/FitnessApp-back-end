using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class PlansOfUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Id плана")]
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public TrainingPlan TrainingPlan { get; set; }
        [Required]
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
