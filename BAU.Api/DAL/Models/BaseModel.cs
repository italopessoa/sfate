using System.ComponentModel.DataAnnotations.Schema;

namespace BAU.Api.DAL.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
