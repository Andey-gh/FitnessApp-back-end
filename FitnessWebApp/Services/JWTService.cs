using FitnessWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Services
{
    public class JWTservice
    {
        private readonly UserManager<User> _userManager;
        public JWTservice(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> CheckUser(string encodedToken)
        {
            var stream = encodedToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            var UserId = tokenS.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            if (UserId == null)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return null;
            }
            return user;
        }

    }
}
