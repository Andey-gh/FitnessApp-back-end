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
        private readonly UserMetricsManager _userMetricsManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signinMgr, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            signInManager = signinMgr;
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _userMetricsManager = new UserMetricsManager(context,userManager);

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
            if (UserId == null) 
            { 
                return Unauthorized(); 
            }

            var user =await _userManager.FindByIdAsync(UserId);
            if(user==null)
            {
                return Unauthorized();
            }

            return Json(_userMetricsManager.GetUserMetrics(user));
       
        }
        [HttpPut]
        [Route("UserMetrics")]
        
        public async Task<IActionResult> UpdateMetrics(UserMetricsUpdateModel UserMetrics)
        {
                if (!ModelState.IsValid)
                {
                    return UnprocessableEntity();
                }

                var UserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                if(UserId==null)
                {
                    return Unauthorized();
                }
                var user = await _userManager.FindByIdAsync(UserId);
                if(user==null)
                {
                    return Unauthorized();
                }

                _userMetricsManager.UpdateUserMetrics(user, UserMetrics);
                return Ok();
            
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
             if(user==null)
             {
                return Unauthorized();
             }
            _userMetricsManager.PostUserMetrics(user, model);
            return Ok();

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
