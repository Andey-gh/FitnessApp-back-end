using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class User: IdentityUser 
    {

        [MaxLength(50)]
        public string Name { get; set; }

        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }

        public int MaxPushUps { get; set; }
        public int MaxPullUps { get; set; }

        [MaxLength(6)]
        public string Gender { get; set; }

        [MaxLength(40)]
        public string Goal { get; set; }
        public bool IsMetrics { get; set; }
        public int ActivePlanId { get; set; }
        [ForeignKey(nameof(ActivePlanId))]
        public TrainingPlan TrainingPlan { get; set; }

    }
}
