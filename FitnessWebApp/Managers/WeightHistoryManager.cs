using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessWebApp.Managers
{
    public class WeightHistoryManager
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;
        public WeightHistoryManager(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> AddChange(WeightHistory history)
        {
            User user = await _userManager.FindByIdAsync(history.UserId);
            if (user!=null) {
                WeightHistory old;
                user.Weight = history.Weight;
                await _userManager.UpdateAsync(user);
                try { old = _context.WeightHistories.First(x => x.UserId == history.UserId && x.Date == history.Date); } catch { old = null;}
                if (old!=null) {
                    old.Weight = history.Weight;
                     _context.WeightHistories.Update(old);
                } else {
                   await _context.WeightHistories.AddAsync(history);           
                }
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }           
        }
    }
}
