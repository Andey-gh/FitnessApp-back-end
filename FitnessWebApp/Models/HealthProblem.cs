using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class HealthProblem
    {
        [Key]
        public int Id { get; set; }

        
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        
        [Display(Name = "Проблема со здоровьем")]
        [MaxLength(50)]
        public string Problem { get; set; }
        public HealthProblem()
        {

        }
        public HealthProblem(string userId,string problem)
        {
            this.UserId = userId;
            this.Problem = problem;
        }
    }
}
