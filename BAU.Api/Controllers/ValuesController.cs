﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BAU.Api.Controllers
{
    [Authorize]
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
        /// <response code="200">Returns a message to indicate if the token is valid</response>
        /// <returns>Settings array</returns>
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
        [HttpGet]
        [Route("settings")]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult Settings()
        {
            return Ok(new
            {
                SHIFT_DURATION = _config["App:SHIFT_DURATION"],
                WEEK_SCAN_PERIOD = _config["App:WEEK_SCAN_PERIOD"],
                MAX_SHIFT_SUM_HOURS_DURATION = _config["App:MAX_SHIFT_SUM_HOURS_DURATION"],
                issuer = _config["Jwt:Issuer"],
                audience = _config["Jwt:Audience"],
                key = _config["Jwt:Key"],
                LifeTimeInMinutes = _config["Jwt:LifeTimeInMinutes"]
            });
        }
    }
}
