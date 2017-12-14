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
        /// Set engineer shift date
        /// </summary>
        /// <param name="engineerId">Engineer Id</param>
        /// <param name="date">Shift date</param>
        void SetEngineerShift(int engineerId, DateTime date);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineerId"></param>
        /// <returns></returns>
        List<EngineerShift> GetEngineerShifts(int engineerId);
    }
}
