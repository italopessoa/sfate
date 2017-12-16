using BAU.Api.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BAU.Test.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DbContextUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        public static DbContextOptions<BAUDbContext> GetContextOptions(string contextName) =>
            new DbContextOptionsBuilder<BAUDbContext>()
                .UseInMemoryDatabase(contextName)
                .Options;
    }
}
