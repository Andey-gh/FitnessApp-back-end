using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessWebApp.Models;
using RestSharp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FitnessWebApp.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using FitnessWebApp.Domain;
using FitnessWebApp.Managers;
using Microsoft.Extensions.Configuration;
using AuthenticationPlugin;

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class AccountController:Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AccountController(UserManager<User> userManager, SignInManager<User> signinMgr, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            signInManager = signinMgr;
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);

        }
        /*[AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }*/


        [HttpGet]
        [Route("UserMetrics")]
        
        public async Task<IActionResult> GetMetrics()
        {
            
            var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            
            if(UserId!=null)
            {
                var user =await _userManager.FindByIdAsync(UserId);
                if(user!=null)
                {
                    var user_health = _context.HealthProblems.Where(x => x.UserId == user.Id).ToList();
                    List<string> health_problems = new List<string>();
                    if (user_health != null)
                    {

                        for (int i = 0; i < user_health.Count; i++)
                        {
                            health_problems.Add(user_health[i].Problem);
                        }
                    }
                    var user_metrics = new UserProfileViewModel() { MetricAge = user.Age, MetricGoal = user.Goal, HealthProblems = health_problems, MetricHeight = user.Height, MetricPullUps = user.MaxPullUps, MetricPushUps = user.MaxPushUps, MetricWeight = user.Weight, Name = user.Name, MetricGender = user.Gender };

                    return Json(user_metrics);
                }
                return Unauthorized();
                
            }
            else
            {
                return Unauthorized();
            }
            
            
        }
        [HttpPut]
        [Route("UserMetrics")]
        
        public async Task<IActionResult> UpdateMetrics(UserMetricsUpdateModel UserMetrics)
        {
            if (ModelState.IsValid)
            {
                var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                
                if (UserId != null)
                {   var user = await _userManager.FindByIdAsync(UserId);
                    if(user!=null)
                    { 
                    user.Name = UserMetrics.Name;
                    WeightHistory history = new WeightHistory(user.Id,UserMetrics.MetricWeight,DateTime.Now.Date);
                    WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
                    await manager.AddChange(history);
                    user.Age = UserMetrics.MetricAge;
                    user.Gender = UserMetrics.MetricGender;
                    user.Goal = UserMetrics.MetricGoal;
                    user.Height = UserMetrics.MetricHeight;
                    for(int i=0;i<UserMetrics.healthProblems.Count;i++)
                    {
                        UserMetrics.healthProblems[i].UserId = user.Id;
                    }
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    var user_health = _context.HealthProblems.Where(x => x.UserId == user.Id).ToList();
                    _context.HealthProblems.RemoveRange(user_health);
                    await _context.SaveChangesAsync();
                    await _context.HealthProblems.AddRangeAsync(UserMetrics.healthProblems);
                    await _context.SaveChangesAsync();

                    return Ok();
                    }
                    return Unauthorized();
                }
                return Unauthorized();
            }
            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.UserLogin);
                if (user != null)
                {
                    
                    var result =_userManager.CheckPasswordAsync(user, model.Password);
                    
                    if (result.Result)
                    {
                        

                        var claims = new[]
                        {
                           new Claim("Email", user.Email),
                           new Claim("Id", user.Id),
                        };
                        var token = _auth.GenerateAccessToken(claims);
                        UserViewModel user_model = new UserViewModel()
                         {
                             Id = user.Id,
                             Name=user.Name,
                             Age=user.Age,
                             Weight=user.Weight,
                             Height=user.Height,
                             Gender=user.Gender,
                             Email=user.Email,
                             isMetrics=user.IsMetrics,
                             ActivePlanId=user.ActivePlanId,
                             AccesToken=token.AccessToken

                         };
                         return Json(user_model);
                        


                    }
                }
                return Unauthorized();
                
            }
            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("sendMetrics")]
        public async Task<IActionResult> PostMetrics(MetricsModel model)
        {
            if (ModelState.IsValid)
            {
                var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                if (UserId == null) 
                { 
                    return Unauthorized(); 
                }
                var user = await _userManager.FindByIdAsync(UserId);
                
                if (user != null)
                {
                   
                    user.Age = model.MetricAge;
                    user.Height = model.MetricHeight;
                    WeightHistory history = new WeightHistory(user.Id, model.MetricWeight, DateTime.Now.Date);
                    WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
                    await manager.AddChange(history);
                    user.Goal = model.MetricGoal;
                    user.MaxPushUps = model.MetricPushUps;
                    user.MaxPullUps = model.MetricPullUps;
                    user.IsMetrics = true;
                    

                   for(int i=0;i<model.MetricHealth.Count;i++)
                    {
                        HealthProblem problem = new HealthProblem(user.Id, model.MetricHealth[i].Problem);
                       
                        await _context.HealthProblems.AddAsync(problem);
                        await _context.SaveChangesAsync();
                    }
                    

                    await _userManager.UpdateAsync(user);
                    return Ok();
                }
                return Unauthorized();
            }
            return UnprocessableEntity();
        }
        [HttpPost]
        
        [Route("ChangeUserActivePlan/{PlanId}")]
        public async Task<IActionResult> ChangeActivePlan(int planId)
        {
            var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            if (UserId == null) 
            { 
                return Unauthorized(); 
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if(user==null)
            {
                return Unauthorized();
            }
            var plan = _context.TrainingPlans.Where(x => x.Id == planId).FirstOrDefault();
            if(plan==null)
            {
                return NoContent();
            }
            user.ActivePlanId = plan.Id;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return Ok();

                
        }
        [HttpGet]
        [Route("refresh_token")]
        public async Task<IActionResult> Logout()
        {
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
           // var token = HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.ToString();
           // token = token.Split(" ").Last();
            var claims = new[]
                        {
                           new Claim("Email", user.Email),
                           new Claim("Id", user.Id),
                        };
            var token = _auth.GenerateAccessToken(claims);

            return Ok(token);
           
        }


    }
}
