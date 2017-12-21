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
    [Authorize]
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
        /// Schedule a date with N engineers
        /// </summary>
        /// <param name="schedule">Schedule request model</param>
        /// <response code="200">Return engineers scheduled for the selected date</response>
        /// <response code="401">JWT is not valid or is null</response>
        /// <response code="400">If the date is a weekend day; If the date value is empty; If the Count value is empty</response>
        /// <returns>List of enginners</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<EngineerModel>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 400)]
        [Route("ScheduleNgineersShift")]
        public IActionResult ScheduleEngineersShift([FromBody] ShiftRequestModel schedule)
        {
            IActionResult response = ValidateRequest(schedule);
            if (response != null)
            {
                return response;
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

        /// <summary>
        /// Schedule a range of dates with N engineers per day
        /// </summary>
        /// <param name="schedule">Schedule request model</param>
        /// <response code="200">Return engineers scheduled for the selected date</response>
        /// <response code="401">JWT is not valid or is null</response>
        /// <response code="400">If the date is a weekend day; If the date value is empty; If the Count value is empty</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<EngineerModel>), 200)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 400)]
        [Route("ScheduleNgineersShiftRange")]
        public IActionResult ScheduleEngineersShiftRange([FromBody] ShiftRequestModel schedule)
        {
            IActionResult response = ValidateRequest(schedule);
            if (response != null)
            {
                return response;
            }
            else
            {
                try
                {
                    _shiftService.ScheduleEngineerShiftRange(schedule);
                    response = Ok("success");
                }
                catch (Exception ex)
                {
                    response = BadRequest(ex.Message);
                }
            }
            return response;
        }

        /// <summary>
        /// Get scheduled shifts
        /// </summary>
        /// <response code="200">Return all shifts</response>
        /// <returns>List of shifts</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("All")]
        [ProducesResponseType(typeof(List<ShiftSummary>), 200)]
        public IActionResult GetShifts()
        {
            List<ShiftSummary> shifts = new List<ShiftSummary>();
            var result = _shiftService.FindAll()
                .GroupBy(item => new
                {
                    item.Date,
                    item.Engineer.Name
                })
                .Select(group => new
                {
                    Date = group.Key.Date.ToShortDateString(),
                    EngineerName = group.Key.Name
                }).OrderBy(x => x.Date);
            result.ToList().ForEach(item =>
            {
                var index = shifts.FindIndex(x => x.Date.Equals(item.Date));
                if (index >= 0)
                {
                    shifts[index].Engineers.Add(item.EngineerName);
                }
                else
                {
                    shifts.Add(new ShiftSummary { Date = item.Date, Engineers = new List<string> { item.EngineerName } });
                }
            });

            return Ok(shifts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IActionResult ValidateRequest(ShiftRequestModel model)
        {
            if (model == null)
                return BadRequest("All values must be informed.");

            IActionResult response = null;
            if (model.StarDate.DayOfWeek == DayOfWeek.Saturday || model.StarDate.DayOfWeek == DayOfWeek.Sunday)
            {
                response = BadRequest("Weekends are not valid working days.");
            }
            else if (model.StarDate == DateTime.MinValue)
            {
                response = BadRequest("Date value cannot be empty.");
            }
            else if (model.StarDate < DateTime.Now.Date)
            {
                response = BadRequest("It is not possible to schedule backward.");
            }
            else if (model.Count == 0)
            {
                response = BadRequest("The number of support engineers is required.");
            }
            else if (model.EndDate > DateTime.MinValue)
            {
                if (model.EndDate <= model.StarDate)
                {
                    response = BadRequest("The final date must be greater than the initial date.");
                }
            }
            return response;
        }

        class ShiftSummary
        {
            public string Date { get; set; }
            public List<string> Engineers { get; set; }
        }
    }
}
