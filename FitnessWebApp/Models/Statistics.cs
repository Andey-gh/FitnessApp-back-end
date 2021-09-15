using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class Statistics
    {
        [Required]
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Периуд")]
        public int Period { get; set; }

        [Display(Name = "Шаг")]
        public string Step { get; set; }

    }
}
