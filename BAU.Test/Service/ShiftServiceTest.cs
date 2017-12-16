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
using AutoMapper;

namespace BAU.Test.Service
{
    public class ShiftServiceTest
    {
        [Fact]
        public void ScheduleEngineerShift_Success()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<BAUMappingProfile>();
            });

            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1"},
                new Engineer{Name = "2"},
                new Engineer{Name = "3"},
                new Engineer{Name = "4"},
                new Engineer{Name = "5"},
                new Engineer{Name = "6"},
                new Engineer{Name = "7"},
                new Engineer{Name = "8"},
                new Engineer{Name = "9"},
                new Engineer{Name = "10"}
            };

            var savedShiftEngineers = new List<EngineerShift>
            {
                new EngineerShift {Engineer = engineers[0], Date = DateTime.Today, Duration = 4},
                new EngineerShift {Engineer = engineers[1], Date = DateTime.Today, Duration = 4}
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(DateTime.Today)).Returns(engineers);
            mockRepository.Setup(s => s.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>())).Returns(savedShiftEngineers);

            IShiftService service = new ShiftService(mockRepository.Object);
            var result = service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, Date = DateTime.Today });
            Assert.Equal(Mapper.Map<List<EngineerShiftModel>>(savedShiftEngineers), result);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Once());
        }

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
            // mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
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
            // mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
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
            // mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineerShift_ExceedsPeriodLimit_Error()
        {
        }
    }
}
