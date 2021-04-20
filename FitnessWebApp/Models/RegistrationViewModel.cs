using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserLogin { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        //убрать
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        
        [Display(Name = "Вес")]
        public int Weight { get; set; }
       
        [Display(Name = "Рост")]
        public int Height { get; set; }
       
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        //убрать
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}
