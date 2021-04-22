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

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    [ApiController]

    public class RegistrationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private AppDbContext _context;

        public RegistrationController(UserManager<User> userMgr, SignInManager<User> signinMgr, AppDbContext Context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            _context = Context;
        }

        [Route("reg")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserLogin,Name = model.Name,Email = model.Email };
                // добавляем пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "api",
                        new { userId = user.Id, code = token },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Confirm E-mail</a>");
                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
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
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                UserViewModel user_model = new UserViewModel(user.Id, user.Age, user.Name, user.Weight, user.Height, user.Gender, user.Email);
                PlansOfUser plansOfUser = new PlansOfUser();
                plansOfUser.PlanId = 1;
                plansOfUser.UserId = user_model.Id;
                await _context.PlansOfUsers.AddAsync(plansOfUser);
                await _context.SaveChangesAsync();
                await signInManager.SignOutAsync();
                return Redirect("/login");
                //return Json(user_model);
            }
            else
                return NotFound("Error");
        }
        
    }
    
}
