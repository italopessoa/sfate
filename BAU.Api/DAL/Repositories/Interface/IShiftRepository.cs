using System;
using System.Collections.Generic;
using BAU.Api.DAL.Models;

namespace BAU.Api.DAL.Repositories.Interface
{
    /// <summary>
    /// Shift repository
    /// </summary>
    public interface IShiftRepository
    {
        /// <summary>
        /// Get a list of available Engineers
        /// </summary>
        /// <param name="shiftDate">Enginneer's schedule date</param>
        /// <returns>List of engineers</returns>
        IList<Engineer> GetEngineersAvailableOn(DateTime shiftDate);

        /// <summary>
        /// Schedule engineer shift date
        /// </summary>
        /// <param name="engineerId">Engineer Id</param>
        /// <param name="date">Shift date</param>
        /// <param name="duration">Shift duration</param>
        void ScheduleEngineerShift(int engineerId, DateTime date, int duration);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineerId"></param>
        /// <returns></returns>
        List<EngineerShift> GetEngineerShifts(int engineerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineerId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<EngineerShift> FindEngineerShifts(int engineerId, DateTime from, DateTime to);
    }
}
