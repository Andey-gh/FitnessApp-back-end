using FitnessWebApp.Models;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FitnessWebApp.Domain
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<TrainingHistory> TrainingHistories { get; set; }
        public DbSet<Excercise> Excercises { get; set; }
        public DbSet<ExcerciseInPlan> ExcercisesInPlan { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<PlansOfUser> PlansOfUsers { get; set; }

        public DbSet<HealthProblem> HealthProblems { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<WeightHistory> WeightHistories { get;set;}

    }
    
}
