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

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    
    [ApiController]

    public class AccountController:Controller
    {
        
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private AppDbContext _context;

        public AccountController(UserManager<User> userMgr, SignInManager<User> signinMgr, AppDbContext context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
           // _httpContext = httpContext;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }


        [HttpGet]
        [Route("profile/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            //var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();
            var user = await userManager.FindByIdAsync(id);
         
            return Json(user);
        }
      
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(model.UserLogin);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        UserViewModel user_model = new UserViewModel(user.Id,user.Age,user.Name,user.Weight,user.Height,user.Gender,user.Email);
                        

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
                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Age = model.MetricAge;
                    user.Height = model.MetricHeight;
                    user.Weight = model.MetricWeight;
                    user.Goal = model.MetricGoal;
                    user.MaxPushUps = model.MetricPushUps;
                    user.MaxPullUps = model.MetricPullUps;

                    foreach (string health in model.MetricHealth) {
                        HealthProblem problem = new HealthProblem { Problem = health, UserId = int.Parse(id) };
                        _context.AddAsync(problem);
                    }
                    _context.SaveChangesAsync();

                    userManager.UpdateAsync(user);
                    return Ok("Metrics were saved");
                }
                return Unauthorized();
            }
            return UnprocessableEntity();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
           
       
            return RedirectToAction("api", "login");
           
        }



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

    }
}
