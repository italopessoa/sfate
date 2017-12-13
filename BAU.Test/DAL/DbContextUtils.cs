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
        /// <returns></returns>
        public static BAUDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BAUDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;
            return new BAUDbContext(options);
        }
    }
}
