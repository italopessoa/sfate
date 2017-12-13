using System;
using System.Collections.Generic;
using BAU.Api.DAL.Contexts;
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
            _contextNames.Add("GetAvailableEngineers_2Engineers_NoEngineers");
            var options = DbContextUtils.GetContextOptions("GetAvailableEngineers_2Engineers_NoEngineers");
            // fake data
            using (var context = new BAUDbContext(options))
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }

            // test
            using (var context = new BAUDbContext(options))
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }

            // clear
            using (var context = new BAUDbContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }

        // [Fact]
        // public void GetAvailableEngineers_2Engineers_OnlyOne()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void GetAvailableEngineers_2Engineers_Success()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void SetEngineerShift_Success()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void SetEngineerShift_ConsecutiveDays_Error()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void SetEngineerShift_WholeDayInPeriod_Error()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void SetEngineerShift_SameDay_Error()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        // [Fact]
        // public void GetEngineerShifts_Success()
        // {
        //     using (var context = DbContextUtils.GetContext())
        //     {
        //         IShiftRepository repository = new ShiftRepository(context);
        //         repository.GetAvailableEngineers(1);
        //     }
        // }

        public void Dispose()
        {
            foreach (string name in _contextNames)
            {
                var options = DbContextUtils.GetContextOptions("GetAvailableEngineers_2Engineers_NoEngineers");
                using (var context = new BAUDbContext(options))
                {
                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
