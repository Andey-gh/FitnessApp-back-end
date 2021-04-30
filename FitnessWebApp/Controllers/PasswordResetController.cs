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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return Unauthorized();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
    
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",$"Код для сброса пароля: {code}");
                return Ok("Confirmation link was sended to your email");
            }
            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
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
