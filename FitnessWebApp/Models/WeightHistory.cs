using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class WeightHistory
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Id пользователя")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Display(Name = "Вес")]
        public int Weight { get; set; }


        [Display(Name = "Время смены веса")]
        public DateTime Date { get; set; }

        public WeightHistory(string UserId, int Weight, DateTime Date) {
            this.UserId = UserId;
            this.Weight = Weight;
            this.Date = Date;
        }


    }
}
