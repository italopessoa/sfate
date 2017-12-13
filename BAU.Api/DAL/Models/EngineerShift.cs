using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAU.Api.DAL.Models
{
    public class EngineerShift : BaseModel
    {
        public int EngineerId { get; set; }
        public Engineer Engineer { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        public byte Duration { get; set; }
    }
}
