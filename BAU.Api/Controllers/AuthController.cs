using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BAU.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="config"></param>
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generate authorization token
        /// </summary>
        /// <param name="loginModel">Login model</param>
        /// <response code="200">Valid user</response>
        /// <returns>Token</returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            IActionResult response = Unauthorized();
            UserModel user = Authenticate(loginModel);
            if (user != null)
            {
                response = Ok(new { token = BuildToken(user) });
            }
            return response;
        }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="user">UserModel</param>
        /// <returns>Token</returns>
        private string BuildToken(UserModel user)
        {
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Check if the user credentials are valid
        /// </summary>
        /// <param name="login"></param>
        /// <returns>UserModel</returns>
        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;
            if (login != null)
            {
                user = new UserModel { UserName = login.Username };
            }

            return user;
        }
    }
}
