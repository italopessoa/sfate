using System;
using BAU.Api.Models;

namespace BAU.Api.Service.Interface
{
    public interface IShiftService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="engineerShiftModel"></param>
        void ScheduleEngineerShift(EngineerShiftModel engineerShiftModel);
    }
}
