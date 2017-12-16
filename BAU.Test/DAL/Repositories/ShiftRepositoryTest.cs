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
using BAU.Api.Utils;

namespace BAU.Test.DAL.Repositories
{
    public class ShiftRepositoryTest : IDisposable
    {
        private IList<string> _contextNames = new List<string>();

        [Theory(DisplayName = "Repository.GetEngineersAvailableOn_Check_Consecutive_Days")]
        [ClassData(typeof(GetEngineersAvailableOn_Check_Consecutive_Days))]
        public void GetEngineersAvailableOn_Check_Consecutive_Days(DateTime date, int nEngineers)
        {
            string contextName = $"GetEngineersAvailableOn_Check_Consecutive_Days";
            _contextNames.Add(contextName);
            DbContextOptions<BAUDbContext> options = DbContextUtils.GetContextOptions(contextName);
            IShiftRepository repository = null;
            const int shiftDuration = 4;

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
                var engineer1 = new Engineer { Name = "1" };
                var engineer2 = new Engineer { Name = "2" };
                var engineer3 = new Engineer { Name = "3" };
                var engineer4 = new Engineer { Name = "4" };
                var engineer5 = new Engineer { Name = "5" };
                var engineer6 = new Engineer { Name = "6" };
                var engineer7 = new Engineer { Name = "7" };
                var engineer8 = new Engineer { Name = "8" };
                var engineer9 = new Engineer { Name = "9" };
                var engineer10 = new Engineer { Name = "10" };
                var engineersList = new List<Engineer>
                {
                    engineer1, engineer2, engineer3, engineer4,
                    engineer5, engineer6, engineer7, engineer8,
                    engineer9, engineer10,
                };

                context.Engineers.AddRange(engineersList);

                var today = new DateTime(2017, 12, 11);
                var shifts = new List<EngineerShift>
                {
                    //first week
                    new EngineerShift { Date = today.Date, Duration = shiftDuration, Engineer = engineer1 },
                    new EngineerShift { Date = today.Date, Duration = shiftDuration, Engineer = engineer2 },
                    new EngineerShift { Date = today.Date.AddDays(1), Duration = shiftDuration, Engineer = engineer7 },
                    new EngineerShift { Date = today.Date.AddDays(2), Duration = shiftDuration, Engineer = engineer3 },
                    new EngineerShift { Date = today.Date.AddDays(2), Duration = shiftDuration, Engineer = engineer4 },
                    new EngineerShift { Date = today.Date.AddDays(4), Duration = shiftDuration, Engineer = engineer5 },
                    new EngineerShift { Date = today.Date.AddDays(4), Duration = shiftDuration, Engineer = engineer6 },
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



        [Theory(DisplayName = "Repository.GetEngineersAvailableOn_Date")]
        [ClassData(typeof(GetEngineersAvailableOn_Date_Generator))]
        public void GetEngineersAvailableOn_Date(DateTime date, int nEngineers)
        {
            string contextName = $"GetEngineersAvailableOn_Date";
            _contextNames.Add(contextName);
            DbContextOptions<BAUDbContext> options = DbContextUtils.GetContextOptions(contextName);
            IShiftRepository repository = null;
            const int shiftDuration = 4;

            // fake data
            using (BAUDbContext context = new BAUDbContext(options))
            {
                var engineer1 = new Engineer { Name = "1" };
                var engineer2 = new Engineer { Name = "2" };
                var engineer3 = new Engineer { Name = "3" };
                var engineer4 = new Engineer { Name = "4" };
                var engineer5 = new Engineer { Name = "5" };
                var engineer6 = new Engineer { Name = "6" };
                var engineer7 = new Engineer { Name = "7" };
                var engineer8 = new Engineer { Name = "8" };
                var engineer9 = new Engineer { Name = "9" };
                var engineer10 = new Engineer { Name = "10" };
                var engineersList = new List<Engineer>
                {
                    engineer1, engineer2, engineer3, engineer4,
                    engineer5, engineer6, engineer7, engineer8,
                    engineer9, engineer10,
                };

                context.Engineers.AddRange(engineersList);

                var today = new DateTime(2017, 12, 11);
                var shifts = new List<EngineerShift>
                {
                    //first week
                    new EngineerShift { Date = today.Date, Duration = shiftDuration, Engineer = engineer1 },
                    new EngineerShift { Date = today.Date, Duration = shiftDuration, Engineer = engineer2 },

                    new EngineerShift { Date = today.Date.AddDays(1), Duration = shiftDuration, Engineer = engineer4 },
                    new EngineerShift { Date = today.Date.AddDays(1), Duration = shiftDuration, Engineer = engineer5 },

                    new EngineerShift { Date = today.Date.AddDays(2), Duration = shiftDuration, Engineer = engineer2 },
                    new EngineerShift { Date = today.Date.AddDays(2), Duration = shiftDuration, Engineer = engineer6 },

                    new EngineerShift { Date = today.Date.AddDays(3), Duration = shiftDuration, Engineer = engineer7 },
                    new EngineerShift { Date = today.Date.AddDays(3), Duration = shiftDuration, Engineer = engineer8 },

                    new EngineerShift { Date = today.Date.AddDays(4), Duration = shiftDuration, Engineer = engineer9 },
                    new EngineerShift { Date = today.Date.AddDays(4), Duration = shiftDuration, Engineer = engineer10 },
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
                new object[] { new DateTime(2017,12,18), 7},
                new object[] { new DateTime(2017,12,19), 9},
                new object[] { new DateTime(2017,12,20), 9},
                new object[] { new DateTime(2017,12,21), 9},
                new object[] { new DateTime(2017,12,22), 9}
            };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class GetEngineersAvailableOn_Check_Consecutive_Days : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
            {
                new object[] { new DateTime(2017,12,11), 7},
                new object[] { new DateTime(2017,12,12), 5},
                new object[] { new DateTime(2017,12,13), 7},
                new object[] { new DateTime(2017,12,14), 6},
                new object[] { new DateTime(2017,12,15), 8},
                new object[] { new DateTime(2017,12,18), 8},
            };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion
}
