using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BAU.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] 
            {
                Environment.GetEnvironmentVariable("DB_SERVER"),
                Environment.GetEnvironmentVariable("DB_CATALOG"),
                Environment.GetEnvironmentVariable("DB_USER"),
                Environment.GetEnvironmentVariable("DB_PASSWORD")
            };
        }
    }
}
