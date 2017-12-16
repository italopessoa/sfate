using System;
using System.Collections.Generic;
using BAU.Api.DAL.Contexts;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using System.Linq;
using BAU.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace BAU.Api.DAL.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ShiftRepository : IShiftRepository
    {
        private readonly BAUDbContext _context;

        /// <summary>
        /// Repository contructor
        /// </summary>
        /// <param name="context"></param>
        public ShiftRepository(BAUDbContext context)
        {
            _context = context;
        }
        public IList<Engineer> GetEngineersAvailableOn(DateTime shiftDate)
        {
            IList<Engineer> engineerShifts = FindEngineersAvailableOn(shiftDate);
            return engineerShifts.Union(_context.Engineers.Include(e => e.Shifts).Where(e => !e.Shifts.Any())).ToList();
        }

        private List<Engineer> FindEngineersAvailableOn(DateTime shiftDate)
        {
            var lastWeek_Monday = shiftDate.PreviousDayOfWeek(DayOfWeek.Monday, 1);
            var endOfWeek = shiftDate.NextDayOfWeek(DayOfWeek.Friday);

            IQueryable<EngineerShift> engineerShifts = FindEngineersShiftsByPeriod(lastWeek_Monday, endOfWeek);
            engineerShifts = FilterEngineerShiftsByConsecutiveShiftDays(engineerShifts, shiftDate);

            return FilterEngineerShiftsByMaxShiftHours(engineerShifts).ToList();
        }
        private IQueryable<EngineerShift> FindEngineersShiftsByPeriod(DateTime from, DateTime to)
        {
            return _context.EngineersShifts.Include(es => es.Engineer)
                .Where(es => from.Date <= es.Date && es.Date <= to.Date);
        }
        private IQueryable<Engineer> FilterEngineerShiftsByMaxShiftHours(IQueryable<EngineerShift> engineerShifts)
        {
            return engineerShifts.GroupBy(es => new { es.Engineer, es.Duration })
                .Where(es => es.Sum(s => s.Duration) < 8)
                .Select(es => es.Key.Engineer);
        }

        private IQueryable<EngineerShift> FilterEngineerShiftsByConsecutiveShiftDays(IQueryable<EngineerShift> engineerShifts, DateTime date)
        {
            DateTime previous = date.DayOfWeek == DayOfWeek.Monday ? date.PreviousDayOfWeek(DayOfWeek.Friday) : date.PreviousBusinessDay();
            return engineerShifts.Where(shift => shift.Date < previous || shift.Date > date.NextBusinessDay());
        }

        public void ScheduleEngineerShift(int engineerId, DateTime date, int duration)
        {
            throw new NotImplementedException();
        }
        public List<EngineerShift> GetEngineerShifts(int engineerId)
        {
            throw new NotImplementedException();
        }

        public List<EngineerShift> FindEngineerShifts(int engineerId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
