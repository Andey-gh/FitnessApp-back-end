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

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    
    [ApiController]

    public class AccountController:Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signinMgr, AppDbContext context)
        {
            _userManager = userManager;
            signInManager = signinMgr;
            _context = context;
           
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }


        [HttpGet]
        [Route("UserMetrics/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMetrics(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var user_health = _context.HealthProblems.Where(x => x.UserId == id).ToList();
                List<string> health_problems = new List<string>();
                if (user_health != null)
                {
                    
                    for (int i = 0; i < user_health.Count; i++)
                    {
                        health_problems.Add(user_health[i].Problem);
                    }
                }
                var user_metrics = new UserProfileViewModel() { MetricAge = user.Age, MetricGoal = user.Goal, HealthProblems = health_problems, MetricHeight = user.Height, MetricPullUps = user.MaxPullUps, MetricPushUps = user.MaxPushUps, MetricWeight = user.Weight, Name = user.Name ,MetricGender=user.Gender};
              
                return Json(user_metrics);
            }
            return Unauthorized();
        }
        [HttpPatch]
        [Route("UserMetrics/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateMetrics(string id,UserMetricsUpdateModel UserMetrics)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Name = UserMetrics.Name;
                    WeightHistory history = new WeightHistory(id,UserMetrics.MetricWeight,DateTime.Now.Date);
                    WeightHistoryManager manager = new WeightHistoryManager(_context, _userManager);
                    await manager.AddChange(history);
                    user.Age = UserMetrics.MetricAge;
                    user.Gender = UserMetrics.MetricGender;
                    user.Goal = UserMetrics.MetricGoal;
                    user.Height = UserMetrics.MetricHeight;
                    for(int i=0;i<UserMetrics.healthProblems.Count;i++)
                    {
                        UserMetrics.healthProblems[i].UserId = id;
                    }
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    var user_health = _context.HealthProblems.Where(x => x.UserId == id).ToList();
                    _context.HealthProblems.RemoveRange(user_health);
                    await _context.SaveChangesAsync();
                   await _context.HealthProblems.AddRangeAsync(UserMetrics.healthProblems);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                return Unauthorized();
            }
            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.UserLogin);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        UserViewModel user_model = new UserViewModel(user.Id,user.Age,user.Name,user.Weight,user.Height,user.Gender,user.Email,user.IsMetrics,user.ActivePlanId);
                        

                        return Json(user_model);
                        
                    }
                }
                return Unauthorized();
                //ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return UnprocessableEntity();
        }

        [HttpPost]
        [AllowAnonymous] //временно,для теста,убрать
        [Route("sendMetrics/{id}")]
        public async Task<IActionResult> PostMetrics(MetricsModel model,string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                
                if (user != null)
                {
                   
                    user.Age = model.MetricAge;
                    user.Height = model.MetricHeight;
                    WeightHistory history = new WeightHistory(id, model.MetricWeight, DateTime.Now.Date);
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
        [AllowAnonymous] 
        [Route("ChangeUserActivePlan/{PlanId}/{UserId}")]
        public async Task<IActionResult> ChangeActivePlan(int planId, string UserId)
        {
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
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
           
       
            return RedirectToAction("api", "login");
           
        }


        #region AuthTest
        /* private async Task Authenticate(string userName)
         {
             // создаем один claim
             var claims = new List<Claim>
     {
         new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
     };
             // создаем объект ClaimsIdentity
             ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
             // установка аутентификационных куки
             await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
         }*/
        /* [HttpPost("Login")]
         public IActionResult Token(LoginViewModel model)
         {
             var identity = GetIdentityAsync(model);
             if (identity == null)
             {
                 return BadRequest(new { errorText = "Invalid username or password." });
             }

             var now = DateTime.UtcNow;
             // создаем JWT-токен
             var jwt = new JwtSecurityToken(
                     issuer: AuthOptions.ISSUER,
                     audience: AuthOptions.AUDIENCE,
                     notBefore: now,
                     claims: HttpContext.User.Claims,
                     expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                     signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
             var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

             var response = new
             {
                 access_token = encodedJwt,
                 username = identity.Id
             };

             return Json(response);
         }

         private async Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
         {
             User user = await userManager.FindByNameAsync(model.UserLogin);

             if (user != null)
             {
                 await signInManager.SignOutAsync();
                 Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                 if(result.Succeeded)
                 { 
                 var claims = new List<Claim>
                 {
                     new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                     new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Name)
                 };
                 ClaimsIdentity claimsIdentity =
                 new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                     ClaimsIdentity.DefaultRoleClaimType);
                 return claimsIdentity;
                 }
             }

             // если пользователя не найдено
             return null;
         }*/
        #endregion

    }
}
