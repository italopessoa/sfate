using System;
using System.Collections.Generic;
using AutoMapper;
using BAU.Api.Controllers;
using BAU.Api.DAL.Repositories.Interface;
using BAU.Api.Models;
using BAU.Api.Service;
using BAU.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace BAU.Test.Controllers
{
    public class ShiftControllerTest
    {
        [Fact]
        public void ScheduleEngineersShift_Date_Weekend_Error()
        {
            Mock<IShiftService> mockService = new Mock<IShiftService>(MockBehavior.Strict);
            ShiftController controller = new ShiftController(mockService.Object);
            var result = controller.ScheduleEngineersShift(new ShiftRequestModel { Count = 1, Date = new DateTime(2017, 12, 17) });
            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(BadRequestObjectResult));
            Assert.Equal("Weekends are not valid working days.", (result as BadRequestObjectResult).Value);
            mockService.Verify(m => m.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineersShift_Date_Empty_Error()
        {
            Mock<IShiftService> mockService = new Mock<IShiftService>(MockBehavior.Strict);
            ShiftController controller = new ShiftController(mockService.Object);
            var result = controller.ScheduleEngineersShift(new ShiftRequestModel { Count = 1 });
            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(BadRequestObjectResult));
            Assert.Equal("Date value cannot be empty.", (result as BadRequestObjectResult).Value);
            mockService.Verify(m => m.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineersShift_Count_Empty_Error()
        {
            Mock<IShiftService> mockService = new Mock<IShiftService>(MockBehavior.Strict);
            ShiftController controller = new ShiftController(mockService.Object);
            var result = controller.ScheduleEngineersShift(new ShiftRequestModel { Date = new DateTime(2017, 12, 18) });
            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(BadRequestObjectResult));
            Assert.Equal("The number of support engineers is required.", (result as BadRequestObjectResult).Value);
            mockService.Verify(m => m.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()), Times.Never);
        }

        [Fact]
        public void ScheduleEngineersShift_Exception()
        {
            Mock<IShiftService> mockService = new Mock<IShiftService>(MockBehavior.Strict);
            mockService.Setup(s => s.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>())).Throws(new InvalidOperationException("Testing controller exception handler"));

            ShiftController controller = new ShiftController(mockService.Object);
            var result = controller.ScheduleEngineersShift(new ShiftRequestModel { Count = 1, Date = new DateTime(2017, 12, 18) });
            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(BadRequestObjectResult));
            Assert.Equal("Testing controller exception handler", (result as BadRequestObjectResult).Value);
            mockService.Verify(m => m.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()), Times.Once());
        }

        [Fact]
        public void ScheduleEngineersShift_Success()
        {
            if(Mapper.Instance == null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<BAUMappingProfile>();
                });
            }
            Mock<IShiftService> mockService = new Mock<IShiftService>(MockBehavior.Strict);
            mockService.Setup(s => s.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()))
            .Returns(
                new List<EngineerShiftModel> {
                    new EngineerShiftModel {Date = DateTime.Now,Duration = 4,Engineer = new EngineerModel{ Id = 1, Name = "Engineer 1"}},
                    new EngineerShiftModel {Date = DateTime.Now,Duration = 4,Engineer = new EngineerModel{ Id = 2, Name = "Engineer 2"}},
                });

            ShiftController controller = new ShiftController(mockService.Object);
            var result = controller.ScheduleEngineersShift(new ShiftRequestModel { Count = 1, Date = new DateTime(2017, 12, 18) });
            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult));
            var expected = new List<EngineerModel> {
                    new EngineerModel { Id = 1, Name = "Engineer 1"},
                    new EngineerModel { Id = 2, Name = "Engineer 2"},
                };
            Assert.Equal(expected, (result as OkObjectResult).Value);
            mockService.Verify(m => m.ScheduleEngineerShift(It.IsAny<ShiftRequestModel>()), Times.Once());
        }
    }
}
