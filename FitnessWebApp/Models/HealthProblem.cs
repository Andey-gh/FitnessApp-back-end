using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class HealthProblem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Id пользователя")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Проблема со здоровьем")]
        [MaxLength(50)]
        public string Problem { get; set; }
    }
}
