using System;

namespace BAU.Api.Models
{
    public class EngineerShiftModel
    {
        public EngineerModel Engineer { get; set; }
        public DateTime Date { get; set; }
        public byte Duration { get; set; }
    }
}
