using BAU.Api.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BAU.Api.DAL.Contexts
{
    /// <summary>
    /// Database context
    /// </summary>
    public class BAUDbContext : DbContext
    {
        /// <summary>
        /// Instatiante BAUDbContext
        /// </summary>
        /// <param name="options">Database context options</param>
        public BAUDbContext(DbContextOptions<BAUDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Engineers
        /// </summary>
        public DbSet<Engineer> Engineers { get; set; }

        /// <summary>
        /// Engineers shifts
        /// </summary>
        public DbSet<EngineerShift> EngineersShifts { get; set; }
    }
}
