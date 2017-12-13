using BAU.Api.DAL.Repositories;
using BAU.Api.DAL.Repositories.Interface;
using Xunit;

namespace BAU.Test.DAL.Repositories
{
    public class ShiftRepositoryTest
    {
        [Fact]
        public void GetAvailableEngineers_2Engineers_NoEngineers()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void GetAvailableEngineers_2Engineers_OnlyOne()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void GetAvailableEngineers_2Engineers_Success()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void SetEngineerShift_Success()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void SetEngineerShift_ConsecutiveDays_Error()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void SetEngineerShift_WholeDayInPeriod_Error()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void SetEngineerShift_SameDay_Error()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }

        [Fact]
        public void GetEngineerShifts_Success()
        {
            using (var context = DbContextUtils.GetContext())
            {
                IShiftRepository repository = new ShiftRepository(context);
                repository.GetAvailableEngineers(1);
            }
        }
    }
}
