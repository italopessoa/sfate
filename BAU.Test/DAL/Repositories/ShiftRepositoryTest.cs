using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BAU.Api.DAL.Contexts;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories;
using BAU.Api.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BAU.Test.DAL.Repositories
{
    public class ShiftRepositoryTest : IDisposable
    {
        private IList<string> _contextNames = new List<string>();

        [Theory(DisplayName = "Repository.GetEngineersAvailableOn_Date")]
        [ClassData(typeof(GetEngineersAvailableOn_Date_Generator))]
        public void GetEngineersAvailableOn_Date(DateTime date, int nEngineers)
        {
            string contextName = "GetEngineersAvailableOn_Today_NoEngineer()";
            _contextNames.Add(contextName);
            DbContextOptions<BAUDbContext> options = DbContextUtils.GetContextOptions(contextName);
            IShiftRepository repository = null;
            const int shiftDuration = 4;

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
                var engineer1 = new Engineer { Name = "Engineer 1" };// completed one whole day
                var engineer2 = new Engineer { Name = "Engineer 2" };// yesterday
                var engineer3 = new Engineer { Name = "Engineer 3" };// today
                var engineer4 = new Engineer { Name = "Engineer 4" };// tomorrow
                var engineersList = new List<Engineer>
                {
                    engineer1,
                    engineer2,
                    engineer3,
                    engineer4,
                };
                context.Engineers.AddRange(engineersList);

                var shifts = new List<EngineerShift>
                {
                    new EngineerShift { Date = DateTime.Today.AddDays(-13), Duration = shiftDuration, Engineer = engineer1 },
                    new EngineerShift { Date = DateTime.Today.AddDays(-6), Duration = shiftDuration, Engineer = engineer1 },
                    new EngineerShift { Date = DateTime.Today.AddDays(-1), Duration = shiftDuration, Engineer = engineer2 },
                    new EngineerShift { Date = DateTime.Today.AddDays(1), Duration = shiftDuration, Engineer = engineer3 },
                    new EngineerShift { Date = DateTime.Today, Duration = shiftDuration, Engineer = engineer4 },
                };

                context.EngineersShifts.AddRange(shifts);
                context.SaveChanges();
            }

            // test
            using (var context = new BAUDbContext(options))
            {
                repository = new ShiftRepository(context);
                IList<Engineer> availableEngineers = repository.GetEngineersAvailableOn(date);
                Assert.Equal(nEngineers, availableEngineers.Count);
            }
        }

        //[Fact]
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

        //[Fact]
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

        //[Fact]
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

        //[Fact]
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

        //[Fact]
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

    #region generator
    class GetEngineersAvailableOn_Date_Generator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {DateTime.Today, 0},
                new object[] { DateTime.Today.AddDays(1), 2}
            };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion
}
