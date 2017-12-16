using System;
using System.Collections.Generic;
using BAU.Api.DAL.Models;
using BAU.Api.DAL.Repositories.Interface;
using BAU.Api.Models;
using BAU.Api.Service;
using BAU.Api.Service.Interface;
using BAU.Api.Utils;
using Moq;
using Xunit;

namespace BAU.Test.Service
{
    public class ShiftServiceTest
    {

        [Fact]
        public void ScheduleEngineerShift_ExceedDayShiftsLimit_Error()
        {
            EngineerShiftModel engineerShiftModel = new EngineerShiftModel
            {
                Date = DateTime.Today,
                Duration = 4,
                Engineer = new EngineerModel { Id = 1 }
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineerShifts(1, engineerShiftModel.Date.PreviousBusinessDay(), engineerShiftModel.Date.NextBusinessDay()))
            .Returns(new List<EngineerShift>
            {
                new EngineerShift { Date = DateTime.Today.Date, Duration = 4, EngineerId = 1 }
            });

            IShiftService service = new ShiftService(mockRepository.Object);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(engineerShiftModel));
            Assert.NotNull(ex);
            Assert.Equal("An engineer can do at most one half day shift in a day.", ex.Message);
            mockRepository.Verify(m => m.FindEngineerShifts(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineerShift_ConsecutiveDays_After_Error()
        {
            EngineerShiftModel engineerShiftModel = new EngineerShiftModel
            {
                Date = DateTime.Today,
                Duration = 4,
                Engineer = new EngineerModel { Id = 1 }
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineerShifts(1, engineerShiftModel.Date.PreviousBusinessDay(), engineerShiftModel.Date.NextBusinessDay()))
            .Returns(new List<EngineerShift>
            {
                new EngineerShift { Date = DateTime.Today.NextBusinessDay(), Duration = 4, EngineerId = 1 }
            });

            IShiftService service = new ShiftService(mockRepository.Object);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(engineerShiftModel));
            Assert.NotNull(ex);
            Assert.Equal("An engineer cannot have half day shifts on consecutive days.", ex.Message);
            mockRepository.Verify(m => m.FindEngineerShifts(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineerShift_ConsecutiveDays_Before_Error()
        {
            EngineerShiftModel engineerShiftModel = new EngineerShiftModel
            {
                Date = DateTime.Today,
                Duration = 4,
                Engineer = new EngineerModel { Id = 1 }
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineerShifts(1, engineerShiftModel.Date.PreviousBusinessDay(), engineerShiftModel.Date.NextBusinessDay()))
            .Returns(new List<EngineerShift>
            {
                new EngineerShift { Date = DateTime.Today.PreviousBusinessDay(), Duration = 4, EngineerId = 1 }
            });

            IShiftService service = new ShiftService(mockRepository.Object);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(engineerShiftModel));
            Assert.NotNull(ex);
            Assert.Equal("An engineer cannot have half day shifts on consecutive days.", ex.Message);
            mockRepository.Verify(m => m.FindEngineerShifts(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineerShift_ExceedsPeriodLimit_Error()
        {
        }
        
        [Fact]
        //[Fact]
        public void ScheduleEngineerShift_Success()
        {
        }
    }
}
