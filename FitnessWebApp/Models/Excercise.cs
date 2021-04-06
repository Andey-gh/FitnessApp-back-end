using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [Display(Name = "Описание упражнения")]
        public string Description { get; set; }
        [Display(Name = "Фото упражнения")]
        public string Photo { get; set; }
       
    }
}
