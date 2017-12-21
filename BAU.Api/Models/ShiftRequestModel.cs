using System;

namespace BAU.Api.Models
{
    public class ShiftRequestModel
    {
        public int Count { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
