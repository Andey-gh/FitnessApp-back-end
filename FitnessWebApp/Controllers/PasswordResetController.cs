using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessWebApp.Services;
using Newtonsoft.Json;

namespace FitnessWebApp.Controllers
{
    [Route("/api")]
    [ApiController]
    public class PasswordResetController : Controller
    {

        private readonly UserManager<User> _userManager;

        public PasswordResetController(UserManager<User> userManager)
        {
            _userManager = userManager;

        }

        [HttpPost]
        [Route("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Json(JsonConvert.SerializeObject(new { Status = "ForgotPasswordConfirmation" }));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "PasswordReset", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",$"Для сброса пароля введите код: {code}");
                return Json(JsonConvert.SerializeObject(new { Status = "ForgotPasswordConfirmation" }));
            }
            return UnprocessableEntity();
        }


        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Ok(model);
        }


    }
}
