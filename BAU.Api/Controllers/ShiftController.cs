using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using BAU.Api.Models;
using BAU.Api.Service.Interface;
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
        private readonly IShiftService _shiftService;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="shiftRepository"></param>
        /// <param name="shiftService"></param>
        public ShiftController(IShiftRepository shiftRepository, IShiftService shiftService)
        {
            _shiftService = shiftService;
            _shiftRepository = shiftRepository;
        }

        /// <summary>
        /// Find available engineers
        /// </summary>
        /// <param name="schedule">Schedule request model</param>
        /// <response code="200">Return engineers scheduled for the selected date</response>
        /// <response code="401">JWT is not valid or is null</response>
        /// <returns>List of enginners</returns>
        // [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<EngineerModel>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 204)]
        [Route("ScheduleNgineersShift")]
        public IActionResult ScheduleEngineersShift([FromBody] ShiftRequestModel schedule)
        {
            IActionResult response = NoContent();
            _shiftService.ScheduleEngineerShift(schedule);
            var engineers = _shiftRepository.FindEngineersAvailableOn(schedule.Date);

            if (engineers.Any())
            {
                response = Ok(new { engineers = Mapper.Map<List<EngineerModel>>(engineers) });
            }
            return response;
        }
    }
}
