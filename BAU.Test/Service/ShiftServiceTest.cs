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
        public ShiftServiceTest()
        {
            Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<BAUMappingProfile>();
                });
        }

        [Fact]
        public void ShiftService_SHIFT_DURATION_Error()
        {
            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration("SHIFT_DURATION")));
            Assert.Equal("App:SHIFT_DURATION", exception.ParamName);
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShift_Success()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1},
                new Engineer{Name = "2", Id = 2},
                new Engineer{Name = "3", Id = 3},
                new Engineer{Name = "4", Id = 4},
                new Engineer{Name = "5", Id = 5},
                new Engineer{Name = "6", Id = 6},
                new Engineer{Name = "7", Id = 7},
                new Engineer{Name = "8", Id = 8},
                new Engineer{Name = "9", Id = 9},
                new Engineer{Name = "10", Id = 10},
            };

            var savedShiftEngineers = new List<EngineerShift>
            {
                new EngineerShift {Engineer = engineers[0], Date = DateTime.Today, Duration = 4},
                new EngineerShift {Engineer = engineers[1], Date = DateTime.Today, Duration = 4}
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(DateTime.Today)).Returns(engineers);
            mockRepository.Setup(s => s.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>())).Returns(savedShiftEngineers);

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            var result = service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, StarDate = DateTime.Today });
            Assert.Equal(Mapper.Map<List<EngineerShiftModel>>(savedShiftEngineers), result);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Once());
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShift_NoMoreEngineers_Error()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1, Shifts = new List<EngineerShift>{new EngineerShift {Date = DateTime.Today}}},
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(DateTime.Today)).Returns(engineers);

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, StarDate = DateTime.Today }));
            Assert.NotNull(ex);
            Assert.Equal("You requested 2 engineers but only 1 is available", ex.Message);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Never());
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShift_ConsecutiveDays_Before_Error()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1, Shifts = new List<EngineerShift>{new EngineerShift {Date = new DateTime(2017, 12, 13)}}},
                new Engineer{Name = "2", Id = 2 },
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(new DateTime(2017, 12, 12))).Returns(engineers);

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, StarDate = new DateTime(2017, 12, 12) }));
            Assert.NotNull(ex);
            Assert.Equal("1: An engineer cannot have half day shifts on consecutive days.", ex.Message);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Never());
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShift_ConsecutiveDays_After_Error()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1, Shifts = new List<EngineerShift>{new EngineerShift {Date = new DateTime(2017, 12, 13)}}},
                new Engineer{Name = "2", Id = 2 },
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(new DateTime(2017, 12, 12))).Returns(engineers);

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, StarDate = new DateTime(2017, 12, 12) }));
            Assert.NotNull(ex);
            Assert.Equal("1: An engineer cannot have half day shifts on consecutive days.", ex.Message);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Never());
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShift_ExceedDayShiftsLimit_Error()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1, Shifts = new List<EngineerShift>{new EngineerShift {Date = new DateTime(2017, 12, 12)}}},
                new Engineer{Name = "2", Id = 2 },
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(new DateTime(2017, 12, 12))).Returns(engineers);

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => service.ScheduleEngineerShift(new ShiftRequestModel { Count = 2, StarDate = new DateTime(2017, 12, 12) }));
            Assert.NotNull(ex);
            Assert.Equal("1: An engineer can do at most one half day shift in a day.", ex.Message);
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Never());
            Mapper.Reset();
        }

        [Fact]
        public void ScheduleEngineerShiftRange_Success()
        {
            var engineers = new List<Engineer>
            {
                new Engineer{Name = "1", Id = 1 },
                new Engineer{Name = "2", Id = 2 },
            };

            Mock<IShiftRepository> mockRepository = new Mock<IShiftRepository>(MockBehavior.Strict);
            mockRepository.Setup(s => s.FindEngineersAvailableOn(It.IsAny<DateTime>())).Returns(engineers);
            mockRepository.Setup(s => s.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>())).Returns(new List<EngineerShift>());

            IShiftService service = new ShiftService(mockRepository.Object, Utils.ConfigurationTestBuilder.GetConfiguration());
            service.ScheduleEngineerShiftRange(new ShiftRequestModel
            {
                StarDate = new DateTime(2017, 12, 20),
                EndDate = new DateTime(2017, 12, 26),
                Count = 2
            });

            for (DateTime date = new DateTime(2017, 12, 20); date <= new DateTime(2017, 12, 26); date = date.NextBusinessDay())
            {
                mockRepository.Verify(m => m.FindEngineersAvailableOn(date), Times.Once());
            }
            mockRepository.Verify(m => m.ScheduleEngineerShift(It.IsAny<List<EngineerShift>>()), Times.Exactly(5));
            Mapper.Reset();
        }
    }
}
