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

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    
    [ApiController]

    public class AccountController:Controller
    {
        
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> userMgr, SignInManager<User> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
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
                        //await Authenticate(model.UserLogin);
                         return Json(user_model);
                        //return Redirect($"profile/{user.Id}");
                        //return Ok(User.Identity.Name);
                    }
                }
                return Unauthorized();
                //ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return UnprocessableEntity();
        }
       

        [Authorize]
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
