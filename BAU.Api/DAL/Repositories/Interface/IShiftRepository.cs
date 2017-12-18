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
        List<Engineer> FindEngineersAvailableOn(DateTime shiftDate);

        /// <summary>
        /// Schedule engineer shift
        /// /// </summary>
        /// <param name="shifts">List of shifts to be scheduled </param>
        /// <returns>Scheduled shifts </returns>
        List<EngineerShift> ScheduleEngineerShift(List<EngineerShift> shifts);

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

        /// <summary>
        /// Find all scheduled shifts
        /// </summary>
        /// <returns>Lilst of Shifts</returns>
        List<EngineerShift> FindAll();
    }
}
