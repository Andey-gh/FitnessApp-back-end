using FitnessWebApp.Domain;
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

        public EndTrainingController(AppDbContext context, UserManager<User> userMgr)
        {

            _context = context;
            _userManager = userMgr;
        }

        [HttpPost("trainingSubmit")]

        public async Task<ActionResult> EndTraining(EndTrainingViewModel trainingSubmit)
        {
            if(ModelState.IsValid)
            {
                //var t = JsonConvert.DeserializeObject<EndTrainingViewModel>(jsonString.ToString());
                // var listOfObjectsResult = Json.Decode<List<EndTrainingViewModel>>(jsonString);
                // string output = JsonConvert.SerializeObject(jsonString);
                //List<EndTrainingViewModel> t = JsonConvert.DeserializeObject<List<EndTrainingViewModel>>(jsonString.ToString());
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
                
             for(int i = 0; i < trainingSubmit.exercises.Count; i++) 
                { 
            TrainingHistory trainingHistory = new TrainingHistory(trainingSubmit.trainingPlanId, trainingSubmit.exercises[i].exerciseId, trainingSubmit.exercises[i].kg, trainingSubmit.exercises[i].quantity, trainingSubmit.exercises[i].startTime, trainingSubmit.exercises[i].endTime, user.Id,trainingSubmit.muscleGroupId);
            await _context.AddAsync(trainingHistory);
            await _context.SaveChangesAsync();
                }
                
            
                return Ok();
            }
            return UnprocessableEntity();


        }


    }
}
