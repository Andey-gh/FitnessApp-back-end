using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using FitnessWebApp.Models;
using FitnessWebApp.Services;
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
    [ApiController]
    public class EndTrainingController : Controller
    {
        private AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITrainingManager _trainingManager;
        private readonly JWTservice _jwtService;

        public EndTrainingController(AppDbContext context, UserManager<User> userMgr, ITrainingManager trainingManager, JWTservice jwtService)
        {
            _context = context;
            _userManager = userMgr;
            _trainingManager = trainingManager;
            _jwtService = jwtService;
        }

        [HttpPost("trainingSubmit")]

        public async Task<ActionResult> EndTraining(EndTrainingViewModel trainingSubmit)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }

            _trainingManager.SubmitTraining(user, trainingSubmit);
            return Ok();

        }


    }
}
