using System.Collections.Generic;
using System.Linq;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAU.Api.Controllers
{
    /// <summary>
    /// Shifts controller
    /// </summary>
    [Route("api/[controller]")]
    public class ShiftController : Controller
    {
        private readonly IEnginnerRepository _enginnerRepository;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="enginnerRepository"></param>
        public ShiftController(IEnginnerRepository enginnerRepository)
        {
            _enginnerRepository = enginnerRepository;
        }
        
        /// <summary>
        /// Find available engineers
        /// </summary>
        /// <param name="count">Number of required engineers</param>
        /// <returns>List of enginners</returns>
        // [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IList<Engineer>), 200)]
        [ProducesResponseType(typeof(string), 204)]
        [Route("RequestSupportEnginners/{count:int}")]
        public IActionResult FindAvailableEngineers(int count)
        {
            IActionResult response = NoContent();
            var engineers = _enginnerRepository.GetAvailableEngineers(count);
            if (engineers.Any())
            {
                response = Ok(new { engineers });
            }
            return response;
        }
    }
}
