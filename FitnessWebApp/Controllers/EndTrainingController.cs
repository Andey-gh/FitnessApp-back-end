using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using Newtonsoft.Json;

namespace FitnessWebApp.Controllers
{
   
    [Route("/api")] 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EndTrainingController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly TrainingManager _trainingManager;

        public EndTrainingController(AppDbContext context, UserManager<User> userMgr)
        {
            _context = context;
            _userManager = userMgr;
            _trainingManager = new TrainingManager(context);
        }

        [HttpPost("trainingSubmit")]

        public async Task<ActionResult> EndTraining(EndTrainingViewModel trainingSubmit)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            if (UserId == null)
            {
                return Unauthorized();
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            _trainingManager.SubmitTraining(user, trainingSubmit);
            return Ok();

        }


    }
}
