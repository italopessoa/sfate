using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAU.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Get system settings
        /// </summary>
        /// <remarks>Get system settings.</remarks>
        /// <response code="200">Returns a array with settings values</response>
        /// <returns>Settings array</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(string[]), 200)]
        public IActionResult Get()
        {
            return Ok(new
            {
                server = Environment.GetEnvironmentVariable("DB_SERVER"),
                databse = Environment.GetEnvironmentVariable("DB_CATALOG"),
                user = Environment.GetEnvironmentVariable("DB_USER"),
                password = Environment.GetEnvironmentVariable("DB_PASSWORD")
            });
        }
    }
}
