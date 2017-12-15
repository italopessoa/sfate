using System;
using System.Collections.Generic;
using BAU.Api.DAL.Contexts;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using System.Linq;
using BAU.Api.Utils;

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
            var previousBusinessDay = shiftDate.PreviousBusinessDay();
            var nextBusinessDay = shiftDate.NextBusinessDay();
            var lastWeek_Monday = shiftDate.PreviousDayOfWeek(DayOfWeek.Monday, 1);
            var endOfWeek = shiftDate.NextDayOfWeek(DayOfWeek.Friday);
            var engineers_shifts = (from enginner in _context.Engineers
                                    join eShift in _context.EngineersShifts
                                    on enginner.Id equals eShift.EngineerId into engineerShiftsTemp
                                    from joinObject in engineerShiftsTemp.DefaultIfEmpty()
                                    where
                                        joinObject == null
                                        ||
                                        (
                                            (joinObject.Date < shiftDate.AddDays(-1) || joinObject.Date > shiftDate.AddDays(1))
                                            &&
                                            (lastWeek_Monday <= joinObject.Date && joinObject.Date <= endOfWeek)
                                        )
                                    select new
                                    {
                                        Enginner = enginner,
                                        ShiftDuration = joinObject != null ? joinObject.Duration : 0,
                                        ShiftDate = joinObject != null ? joinObject.Date : DateTime.MinValue
                                    }).ToList();

            List<Engineer> availableEngineers = (from eng in engineers_shifts
                                                 group eng by new { eng.Enginner } into grp
                                                 where grp.Sum(shift => shift.ShiftDuration) < 8
                                                 from eng in grp
                                                 select eng.Enginner).ToList();

            return availableEngineers;
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
