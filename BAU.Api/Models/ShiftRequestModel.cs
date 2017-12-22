using System;

namespace BAU.Api.Models
{
    public class ShiftRequestModel
    {
        /// <summary>
        /// Number of required engineers
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Schedule date or Range Date (if making a range request)
        /// </summary>
        /// <returns></returns>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End Date. Required only for Range requests.
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
