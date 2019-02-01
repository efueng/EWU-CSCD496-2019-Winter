using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GroupControllerTests
    {
        private AutoMocker Mocker { get; set; }
        private Mock<IGroupService> GroupServiceMock { get; set; }
        private GroupController GroupControllerMock { get; set; }

        private DTO.Group MakeMockDTO(string groupName = "Cat Videos", int groupId = 42)
        {
            var dto = Mocker.CreateInstance<DTO.Group>();
            dto.Id = groupId;
            dto.Name = groupName;

            return dto;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Mocker = new AutoMocker();
            GroupServiceMock = Mocker.GetMock<IGroupService>();
            GroupControllerMock = new GroupController(GroupServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupController_RequiresUserService()
        {
            new GroupController(null);
        }

        [TestMethod]
        public void AddGroup_NullGroup_ReturnsBadRequestObjectResult()
        {
            var result = GroupControllerMock.AddGroup(null);
            Assert.IsTrue(result is BadRequestResult);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void AddGroup_MockGroup_ReturnsOkResult()
        {
            // Arrange
            var groupMock = MakeMockDTO();

            // Act
            var result = GroupControllerMock.AddGroup(groupMock);

            // Assert
            Assert.IsTrue(result is OkResult);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void UpdateGroup_NullGroup_ReturnsBadRequestResutl()
        {
            // Arrange
            var mocker = new AutoMocker(MockBehavior.Strict);
            var service = mocker.GetMock<IGroupService>();
            var controller = new GroupController(service.Object);

            service.Setup(x => x.UpdateGroup(It.IsAny<Domain.Models.Group>())).Verifiable();

            // Act
            var result = controller.UpdateGroup(null);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
            service.Verify(x => x.UpdateGroup(null), Times.Never);
        }

        [TestMethod]
        public void UpdateGroup_MockGroup_ReturnsOkResult()
        {
            // Arrange
            var dtoGroup = MakeMockDTO();
            
            // Act
            var result = GroupControllerMock.UpdateGroup(dtoGroup);

            // Assert
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void RemoveGroup_NullGroup_ReturnsBadRequestResult()
        {
            // Arrange
            var mocker = new AutoMocker(MockBehavior.Strict);
            var service = mocker.GetMock<IGroupService>();
            var controller = new GroupController(service.Object);

            service.Setup(x => x.RemoveGroup(It.IsAny<Domain.Models.Group>())).Verifiable();

            // Act
            var result = controller.RemoveGroup(null);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
            service.Verify(x => x.UpdateGroup(null), Times.Never);
        }

        [TestMethod]
        public void FetchAll_ReturnsListOfGroups()
        {
            // Arrange
            var mocker = new AutoMocker(MockBehavior.Strict);
            var service = mocker.GetMock<IGroupService>();
            var controller = new GroupController(service.Object);

            service.Setup(x => x.FetchAll())
                .Returns(new List<Domain.Models.Group>())
                .Verifiable();

            // Act
            var result = controller.FetchAll();

            // Assert
            Assert.IsTrue(result.Value is List<DTO.Group>);
            mocker.VerifyAll();
        }
    }
}
