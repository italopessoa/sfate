using System;
using System.Collections.Generic;
using System.Linq;
using BAU.Api.DAL.Contexts;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories;
using BAU.Api.DAL.Repositories.Interface;
using Xunit;

namespace BAU.Test.DAL.Repositories
{
    public class ShiftRepositoryTest : IDisposable
    {
        private IList<string> _contextNames = new List<string>();

        [Fact]
        public void GetAvailableEngineers_2Engineers_NoEngineers()
        {
            string contextName = "GetAvailableEngineers_2Engineers_NoEngineers";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
                var engineer1 = new Engineer { Name = "Engineer 1" };
                var engineer2 = new Engineer { Name = "Engineer 2" };
                var engineersList = new List<Engineer> 
                {
                    engineer1,
                    engineer2,
                };

                context.Engineers.AddRange(engineersList);
                context.SaveChanges();
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

         [Fact]
        public void GetEngineersAvailableOn_All_NoEngineers()
        {
            string contextName = "GetAvailableEngineersAt_2Engineers_NoEngineers";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
                var engineer1 = new Engineer { Name = "Engineer 1" };
                var engineer2 = new Engineer { Name = "Engineer 2" };
                var engineersList = new List<Engineer> 
                {
                    engineer1,
                    engineer2,
                };

                context.Engineers.AddRange(engineersList);
                context.SaveChanges();
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }


        [Fact]
        public void SetEngineerShift_Success()
        {
            string contextName = "SetEngineerShift_Success";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

        [Fact]
        public void SetEngineerShift_ConsecutiveDays_Error()
        {
            string contextName = "SetEngineerShift_ConsecutiveDays_Error";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

        [Fact]
        public void SetEngineerShift_WholeDayInPeriod_Error()
        {
            string contextName = "SetEngineerShift_WholeDayInPeriod_Error";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

        [Fact]
        public void SetEngineerShift_SameDay_Error()
        {
            string contextName = "SetEngineerShift_SameDay_Error";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

        [Fact]
        public void GetEngineerShifts_Success()
        {
            string contextName = "GetAvailableEngineers_2Engineers_NoEngineers";
            _contextNames.Add(contextName);
            var options = DbContextUtils.GetContextOptions(contextName);

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
            }

            // test
            using (var context = new BAUDbContext(options))
            {
            }
        }

        public void Dispose()
        {
            foreach (string contextName in _contextNames)
            {
                var options = DbContextUtils.GetContextOptions(contextName);
                using (var context = new BAUDbContext(options))
                {
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
