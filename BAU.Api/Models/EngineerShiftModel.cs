using System;

namespace BAU.Api.Models
{
    public class EngineerShiftModel
    {
        public EngineerModel Engineer { get; set; }
        public DateTime Date { get; set; }
        public byte Duration { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (this.Date != (obj as EngineerShiftModel).Date
           || (this.Duration != (obj as EngineerShiftModel).Duration)
           || (!this.Engineer.Equals((obj as EngineerShiftModel).Engineer)))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Date.GetHashCode();
                hash = hash * 23 + Duration.GetHashCode();
                if (Engineer != null)
                {
                    hash = hash * 23 + Engineer.GetHashCode();
                }
                return hash;
            }
        }
    }
}
