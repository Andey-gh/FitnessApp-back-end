using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Controllers
{

    [Route("/api")]
    [ApiController]
    public class EndTrainingController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> userManager;

        public EndTrainingController(AppDbContext context, UserManager<User> userMgr)
        {

            _context = context;
            userManager = userMgr;
        }

        [HttpPost("trainingSubmit/{id}")]

        public async Task<ActionResult> EndTraining(EndTrainingViewModel trainingSubmit, string Id)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(Id);
                {
                    if (user == null)
                        return Unauthorized();
                }
                for (int i = 0; i < trainingSubmit.exercises.Count; i++)
                {
                    TrainingHistory trainingHistory = new TrainingHistory(trainingSubmit.trainingPlanId, trainingSubmit.exercises[i].exerciseId, trainingSubmit.exercises[i].kg, trainingSubmit.exercises[i].quantity, trainingSubmit.exercises[i].startTime, trainingSubmit.exercises[i].endTime, user.Id, trainingSubmit.muscleGroupId);
                    await _context.AddAsync(trainingHistory);
                    await _context.SaveChangesAsync();
                }


                return Ok();
            }
            return UnprocessableEntity();


        }


    }
}
