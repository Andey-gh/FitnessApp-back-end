using FitnessWebApp.Models;
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
        void UpdateUserMetrics(User user, UserMetricsUpdateModel UserMetrics);
        void PostUserMetrics(User user, MetricsModel model);
        Task<IActionResult> ChangeActivePlan(int planId, User user);
    }
}
