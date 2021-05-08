using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class TrainingPlan
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Discription { get; set; }
        [Required]
        [Display(Name = "Название")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Сложность")]
        public int Difficulty { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public float Price { get; set; }
        
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }

        [Display(Name = "Ссылка на фото плана")]
        public string Photo { get; set; }
        
        [Display(Name = "Категория")]
        public string Category { get; set; }

        
    }
}
