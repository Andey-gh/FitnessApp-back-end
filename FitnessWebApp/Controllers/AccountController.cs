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
    [ApiController]

    public class AccountController:Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AuthService _auth;
        private readonly IUserMetricsManager _userMetricsManager;
        private readonly JWTservice _jwtService;

        public AccountController(UserManager<User> userManager, IConfiguration configuration, JWTservice jwtService, IUserMetricsManager userMetricsManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _userMetricsManager = userMetricsManager;
            _jwtService = jwtService;

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
            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
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

            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }

            _userMetricsManager.UpdateUserMetrics(user, UserMetrics,_userManager);
                return Ok();
            
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            { 
                return UnprocessableEntity(); 
            }
                User user = await _userManager.FindByNameAsync(model.UserLogin);
            if (user == null)
            { 
                return Unauthorized(); 
            }
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
                     

                 };
                Response.Cookies.Append("JWT", token.AccessToken.ToString(), new CookieOptions {
                    HttpOnly=true,
                    SameSite=SameSiteMode.None,
                    Secure=true
                    
                });
                 return Json(user_model);

            }
            return Unauthorized();
               
                
            
        }

        [HttpPost]
        [Route("sendMetrics")]
        public async Task<IActionResult> PostMetrics(MetricsModel model)
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
            await _userMetricsManager.PostUserMetrics(user, model,_userManager);
            return Ok();

        }
        [HttpPost]
        
        [Route("ChangeUserActivePlan/{PlanId}")]
        public async Task<IActionResult> ChangeActivePlan(int planId)
        {
            if(!ModelState.IsValid)
             {
                return UnprocessableEntity();
            }

            var user = await _jwtService.CheckUser(Request.Cookies["JWT"]);
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _userMetricsManager.ChangeActivePlan(planId, user,_userManager);

                
        }
        [HttpGet]
        [Route("refresh_token")]
        public async Task<IActionResult> RefreshToken()
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
            var claims = new[]
                        {
                           new Claim("Email", user.Email),
                           new Claim("Id", user.Id),
                        };
            var token = _auth.GenerateAccessToken(claims);

            return Ok(token);
           
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<ActionResult> Logout ()
        {
            if (Request.Cookies["JWT"] != null)
            {
                Response.Cookies.Delete("JWT");
                return Ok();
            }
            return Unauthorized();
        }


    }
}
