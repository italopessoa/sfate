using System;
using System.Collections.Generic;
using BAU.Api.Models;

namespace BAU.Api.Service.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShiftService
    {
        /// <summary>
        /// Find all scheduled shifts
        /// </summary>
        /// <returns>Lilst of Shifts</returns>
        List<EngineerShiftModel> FindAll();

        /// <summary>
        /// Schedule engineer shifts
        /// </summary>
        /// <param name="shiftRequest">Schedule details (number of engineers and date) </param>
        /// <returns>Scheduled egineers </returns>
        List<EngineerShiftModel> ScheduleEngineerShift(ShiftRequestModel shiftRequest);

        /// <summary>
        /// Schedule a range of dates with N engineers per day
        /// </summary>
        /// <param name="shiftRequest">Schedule details (number of engineers and date) </param>
        void ScheduleEngineerShiftRange(ShiftRequestModel shiftRequest);
    }
}
