using System;
using System.Collections.Generic;
using BAU.Api.DAL.Contexts;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using System.Linq;
using BAU.Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BAU.Api.DAL.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ShiftRepository : IShiftRepository
    {
        private readonly BAUDbContext _context;

        /// <summary>
        /// Maximum amount of hours of an engineer
        /// </summary>
        private readonly int MAX_SHIFT_SUM_HOURS_DURATION;

        /// <summary>
        /// Number of weeks to consider while filtering shifts
        /// </summary>
        private readonly int WEEK_SCAN_PERIOD;

        /// <summary>
        /// Repository contructor
        /// </summary>
        /// <param name="context"></param>
        public ShiftRepository(BAUDbContext context, IConfiguration config)
        {
            if (String.IsNullOrEmpty(config["MAX_SHIFT_SUM_HOURS_DURATION"]))
            {
                throw new ArgumentNullException("MAX_SHIFT_SUM_HOURS_DURATION");
            }

            if (String.IsNullOrEmpty(config["WEEK_SCAN_PERIOD"]))
            {
                throw new ArgumentNullException("WEEK_SCAN_PERIOD");
            }

            this.MAX_SHIFT_SUM_HOURS_DURATION = int.Parse(config["MAX_SHIFT_SUM_HOURS_DURATION"]);
            this.WEEK_SCAN_PERIOD = int.Parse(config["WEEK_SCAN_PERIOD"]);
            _context = context;
        }
        public List<Engineer> FindEngineersAvailableOn(DateTime shiftDate)
        {
            IList<Engineer> engineerShifts = FilterEngineersAvailableOn(shiftDate).ToList();
            return engineerShifts.Union(_context.Engineers.Include(e => e.Shifts).Where(e => !e.Shifts.Any())).ToList();
        }

        private IQueryable<Engineer> FilterEngineersAvailableOn(DateTime shiftDate)
        {
            var lastWeek_Monday = shiftDate.PreviousDayOfWeek(DayOfWeek.Monday, this.WEEK_SCAN_PERIOD);
            var endOfWeek = shiftDate.NextDayOfWeek(DayOfWeek.Friday);

            IQueryable<EngineerShift> engineerShifts = FilterEngineersShiftsByPeriod(lastWeek_Monday, endOfWeek);
            engineerShifts = FilterEngineerShiftsByMaxShiftHours(engineerShifts);
            engineerShifts = FilterEngineerShiftsByConsecutiveShiftDays(engineerShifts, shiftDate);
            return engineerShifts.Select(x => x.Engineer);
        }

        public List<EngineerShift> ScheduleEngineerShift(int engineerId, DateTime date, int duration)
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

        #region filters

        /// <summary>
        /// Filter EngineersShifts by period
        /// </summary>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <returns>EngineerShifts between from-end period</returns>
        private IQueryable<EngineerShift> FilterEngineersShiftsByPeriod(DateTime from, DateTime to)
        {
            return _context.EngineersShifts.Include(es => es.Engineer)
                .Where(es => from.Date <= es.Date && es.Date <= to.Date);
        }

        /// <summary>
        /// Filter EngineerShifts by max shift hours
        /// </summary>
        /// <param name="engineerShifts">EngineerShifts</param>
        /// <returns>EngineerShifts with sum of all shifts less than the max time allowed</returns>
        private IQueryable<EngineerShift> FilterEngineerShiftsByMaxShiftHours(IQueryable<EngineerShift> engineerShifts)
        {
            var engineersWithMaxShiftHours = engineerShifts.GroupBy(es => new { es.Engineer.Id, es.Duration })
                .Where(es => es.Sum(s => s.Duration) < this.MAX_SHIFT_SUM_HOURS_DURATION)
                .Select(es => es.Key.Id).ToList();
            return engineerShifts.Where(e => engineersWithMaxShiftHours.Contains(e.EngineerId));
        }

        /// <summary>
        /// Filter EngineerShifts by consecutive shift days
        /// </summary>
        /// <param name="engineerShifts">EngineerShifts</param>
        /// <param name="date">Target date</param>
        /// <returns>EngineerShifts that are on non-consecutive days based on target date</returns>
        private IQueryable<EngineerShift> FilterEngineerShiftsByConsecutiveShiftDays(IQueryable<EngineerShift> engineerShifts, DateTime date)
        {
            DateTime previous = date.DayOfWeek == DayOfWeek.Monday ? date.PreviousDayOfWeek(DayOfWeek.Friday) : date.PreviousBusinessDay();
            DateTime next = date.DayOfWeek == DayOfWeek.Friday ? date.NextDayOfWeek(DayOfWeek.Monday) : date.NextBusinessDay();
            return engineerShifts.Where(shift => shift.Date < previous || shift.Date > next);
        }

        public List<EngineerShift> ScheduleEngineerShift(List<EngineerShift> shifts)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
