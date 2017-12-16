using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using BAU.Api.Models;
using BAU.Api.Service.Interface;
using BAU.Api.Utils;
using Microsoft.Extensions.Configuration;

namespace BAU.Api.Service
{
    public class ShiftService : IShiftService
    {
        private const int MAX_DAY_SHIFT_HOURS = 4;
        private readonly IShiftRepository _repository;
        public ShiftService(IShiftRepository repository)
        {
            _repository = repository;
        }
        public void ScheduleEngineerShift(EngineerShiftModel engineerShiftModel)
        {
            List<EngineerShift> shifts = _repository.FindEngineerShifts(
                    engineerShiftModel.Engineer.Id,
                    engineerShiftModel.Date.PreviousBusinessDay(),
                    engineerShiftModel.Date.NextBusinessDay()
                );
            int totalShiftDay = shifts.Where(s => s.Date == engineerShiftModel.Date).Sum(s => s.Duration);
            if (totalShiftDay + engineerShiftModel.Duration > MAX_DAY_SHIFT_HOURS)
            {
                throw new InvalidOperationException("An engineer can do at most one half day shift in a day.");
            }
            if (shifts.Where(s => s.Date == engineerShiftModel.Date.PreviousBusinessDay()
                || s.Date == engineerShiftModel.Date.NextBusinessDay()).Any())
            {
                throw new InvalidOperationException("An engineer cannot have half day shifts on consecutive days.");
            }

            throw new NotImplementedException();
        }

        public List<EngineerShiftModel> ScheduleEngineerShift(ShiftRequestModel shiftRequest)
        {
            List<Engineer> engineers = _repository.FindEngineersAvailableOn(shiftRequest.Date);
            var randomEngineers = engineers.OrderBy(x => RandomUtil.Rand.Next()).Take(2).ToList();

            List<EngineerShift> shifts = randomEngineers.Select(e =>
                new EngineerShift
                {
                    Date = shiftRequest.Date,
                    EngineerId = e.Id,
                    Duration = 4
                }
            ).ToList();

            return Mapper.Map<List<EngineerShiftModel>>(_repository.ScheduleEngineerShift(shifts));
        }
    }
}
