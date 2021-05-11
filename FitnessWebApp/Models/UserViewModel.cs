using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        
        public string Gender { get; set; }
        public string Email { get; set; }
        public bool isMetrics { get; set; }
        public int ActivePlanId { get; set; }

        public UserViewModel(string Id, int Age, string Name,int Weight,int Height,string Gender,string Email,bool IsMetrics,int activePlanId)
        {
            
            this.Id = Id;
            this.Name = Name;
            this.Age = Age;
            this.Weight = Weight;
            this.Height = Height;
            this.Gender = Gender;
            this.Email = Email;
            this.isMetrics = IsMetrics;
            this.ActivePlanId = activePlanId;
            
        }
    }
}
