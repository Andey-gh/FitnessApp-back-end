using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class Muscle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название мышцы")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Фото мышцы")]       

        public string Photo { get; set; }
    }
}
