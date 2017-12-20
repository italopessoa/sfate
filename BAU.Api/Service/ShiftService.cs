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
        private readonly byte SHIFT_DURATION;
        private readonly IShiftRepository _repository;
        public ShiftService(IShiftRepository repository, IConfiguration config)
        {
            if (String.IsNullOrEmpty(config["App:SHIFT_DURATION"]))
            {
                throw new ArgumentNullException("App:SHIFT_DURATION");
            }
            SHIFT_DURATION = byte.Parse(config["App:SHIFT_DURATION"]);
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

        private void ValidateEngineers(List<Engineer> engineers, DateTime shiftDate)
        {
            engineers.ForEach(eng =>
            {
                if (eng.Shifts.FirstOrDefault(s => s.Date == shiftDate) != null)
                {
                    throw new InvalidOperationException($"{eng.Name}: An engineer can do at most one half day shift in a day.");
                }
                else if (eng.Shifts.Where(s => s.Date == shiftDate.PreviousBusinessDay()
                || s.Date == shiftDate.NextBusinessDay()).Any())
                {
                    throw new InvalidOperationException($"{eng.Name}: An engineer cannot have half day shifts on consecutive days.");
                }
            });
        }

        public List<EngineerShiftModel> ScheduleEngineerShift(ShiftRequestModel shiftRequest)
        {
            List<Engineer> engineers = _repository.FindEngineersAvailableOn(shiftRequest.Date);
            if (engineers.Count < shiftRequest.Count)
            {
                throw new InvalidOperationException
                ($"You requested {shiftRequest.Count} engineer{(shiftRequest.Count > 1 ? "s" : "")} but only {engineers.Count} {(engineers.Count > 1 ? "are" : "is")} available");
            }

            ValidateEngineers(engineers, shiftRequest.Date);
            var randomEngineers = engineers.OrderBy(x => new Random().Next()).Take(shiftRequest.Count).ToList();
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

        public List<EngineerShiftModel> FindAll()
        {
            return Mapper.Map<List<EngineerShiftModel>>(_repository.FindAll());
        }
    }
}
