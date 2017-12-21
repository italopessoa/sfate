using System;

namespace BAU.Api.Models
{
    public class ShiftRequestModel
    {
        public int Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
