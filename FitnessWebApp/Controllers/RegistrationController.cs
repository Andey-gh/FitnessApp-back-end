using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessWebApp.Models;
using FitnessWebApp.Services;
using FitnessWebApp.Domain;
using Microsoft.Extensions.Configuration;

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    [ApiController]

    public class RegistrationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public RegistrationController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [Route("reg")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserLogin,Name = model.Name,Email = model.Email,IsMetrics=false };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "api",
                        new { userId = user.Id, code = token },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService(_configuration);
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Confirm E-mail</a>");
                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                }
                else
                {
                    return StatusCode(501);
                }
            }
            return UnprocessableEntity();
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return NotFound("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
               // UserViewModel user_model = new UserViewModel(user.Id, user.Age, user.Name, user.Weight, user.Height, user.Gender, user.Email);
                PlansOfUser plansOfUser = new PlansOfUser();
                plansOfUser.PlanId = 1;
                plansOfUser.UserId = user.Id;
                await _context.PlansOfUsers.AddAsync(plansOfUser);
                await _context.SaveChangesAsync();
                await _signInManager.SignOutAsync();
                return Redirect("/login");
                //return Json(user_model);
            }
            else return StatusCode(501);
        }
        
    }
    
}
