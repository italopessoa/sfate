using System;
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
        private readonly IShiftRepository _shiftRepository;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="shiftRepository"></param>
        public ShiftController(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        /// <summary>
        /// Find available engineers
        /// </summary>
        /// <param name="schedule">Schedule request model</param>
        /// <returns>List of enginners</returns>
        // [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(IList<Engineer>), 200)]
        [ProducesResponseType(typeof(string), 204)]
        [Route("ScheduleNgineersShift")]
        public IActionResult ScheduleEngineersShift([FromBody] ScheduleModel schedule)
        {
            IActionResult response = NoContent();
            var engineers = _shiftRepository.FindEngineersAvailableOn(schedule.Date);
            if (engineers.Any())
            {
                response = Ok(new { engineers });
            }
            return response;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleModel
    {
        /// <summary>
        /// Number of required N-gineers
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// Shift date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
