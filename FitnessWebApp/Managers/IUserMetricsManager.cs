using FitnessWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public interface IUserMetricsManager
    {
        UserProfileViewModel GetUserMetrics(User user);
        Task<IActionResult> UpdateUserMetrics(User user, UserMetricsUpdateModel UserMetrics, UserManager<User> _userManager);
        Task<IActionResult> PostUserMetrics(User user, MetricsModel model, UserManager<User> _userManager);
        Task<IActionResult> ChangeActivePlan(int planId, User user, UserManager<User> _userManager);
    }
}
