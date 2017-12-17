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
        private readonly IShiftService _shiftService;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="shiftService"></param>
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        /// <summary>
        /// Find available engineers
        /// </summary>
        /// <param name="schedule">Schedule request model</param>
        /// <response code="201">Return engineers scheduled for the selected date</response>
        /// <response code="401">JWT is not valid or is null</response>
        /// <response code="400">If the date is a weekend day; If the date value is empty; If the Count value is empty</response>
        /// <returns>List of enginners</returns>
        //[Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IList<EngineerModel>), 201)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 400)]
        [Route("ScheduleNgineersShift")]
        public IActionResult ScheduleEngineersShift([FromBody] ShiftRequestModel schedule)
        {
            IActionResult response = NoContent();
            if (schedule.Date.DayOfWeek == DayOfWeek.Saturday || schedule.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                response = BadRequest("Weekends are not valid working days.");
            }
            else if (schedule.Date == DateTime.MinValue)
            {
                response = BadRequest("Date value cannot be empty.");
            }
            else if (schedule.Count == 0)
            {
                response = BadRequest("The number of support engineers is required.");
            }
            else
            {
                try
                {
                    var scheduledEngineers = _shiftService.ScheduleEngineerShift(schedule).Select(x => x.Engineer).ToList();
                    if (scheduledEngineers.Any())
                    {
                        response = Ok(Mapper.Map<List<EngineerModel>>(scheduledEngineers));
                    }
                }
                catch (Exception ex)
                {
                    response = BadRequest(ex.Message);
                }
            }
            return response;
        }
    }
}
