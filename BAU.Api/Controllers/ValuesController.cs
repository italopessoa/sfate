using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BAU.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Check JWT
        /// </summary>
        /// <response code="200">Returns a message to indicate that the token is valid</response>
        /// <returns>Settings array</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(string[]), 200)]
        public IActionResult Get()
        {
            return Ok("Authorized");
        }

        /// <summary>
        /// Get system settings
        /// </summary>
        /// <remarks>Get system settings.</remarks>
        /// <response code="200">Returns a array with settings values</response>
        /// <returns>Settings array</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("settings")]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult Settings()
        {
            return Ok(new
            {
                SHIFT_DURATION = Environment.GetEnvironmentVariable("SHIFT_DURATION"),
                MAX_SHIFTS_DURATION = Environment.GetEnvironmentVariable("MAX_SHIFTS_DURATION"),
                issuer = _config["Jwt:Issuer"],
                audience = _config["Jwt:Audience"],
                key = _config["Jwt:Key"]
            });
        }
    }
}
