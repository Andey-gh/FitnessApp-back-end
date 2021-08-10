using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class ExcercisesManager
    {
        private readonly AppDbContext _context;
        public ExcercisesManager(AppDbContext context)
        {
            _context = context;
        }

        public async void AddExcercise(Excercise excercise)
        {
            await _context.AddAsync(excercise);
            await _context.SaveChangesAsync();
            
        }
        public async Task<List<Excercise>> GetExcercises()
        {
            return await _context.Excercises.ToListAsync();
        }
        
        public async void DeleteExcercise(Excercise excercise)
        {
            
            _context.Excercises.Remove(excercise);
            await _context.SaveChangesAsync();
        }
    }
}
