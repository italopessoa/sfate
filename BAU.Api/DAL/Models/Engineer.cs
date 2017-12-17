using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAU.Api.DAL.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Engineer : BaseModel
    {
        public Engineer()
        {
            Shifts = new List<EngineerShift>();
        }
        /// <summary>
        /// Engineer's name
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<EngineerShift> Shifts { get; set; }
    }
}
