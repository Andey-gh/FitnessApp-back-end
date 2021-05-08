using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class Excercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название упражнения")]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Display(Name = "Описание упражнения")]
        public string Description { get; set; }
       
        public int? TargetMuscleId { get; set; }
        [ForeignKey(nameof(TargetMuscleId))]
        public virtual Muscle TargetMuscle { get; set; }
        public int? AssistantMuscleId { get; set; }
        
        [ForeignKey(nameof(AssistantMuscleId))]
        public virtual Muscle AssistantMuscle { get; set; } 

        [Display(Name = "Фото упражнения")]

        public string Photo { get; set; }

    }
}
