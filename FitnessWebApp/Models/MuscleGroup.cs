using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class MuscleGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название группы мышц")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Display(Name = "Фото группы мышц")]
        public string Photo { get; set; }
    }
}
