using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private AutoMocker Mocker { get; set; }
        private Mock<IUserService> UserServiceMock { get; set; }
        private Mock<UserController> UserControllerMock { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Mocker = new AutoMocker();
            UserServiceMock = Mocker.GetMock<IUserService>();
            //UserControllerMock = new UserController(UserServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserController_RequiresUserService()
        {
            new UserController(null);
        }

        [TestMethod]
        public void AddUser_NullUser_ReturnsBadRequestResult()
        {
            var controller = new UserController(UserServiceMock.Object);
            UserServiceMock.Setup(x => x.AddUser(It.IsAny<User>())).Verifiable();

            var result = controller.AddUser(null);
            Assert.IsTrue(result is BadRequestResult);

            UserServiceMock.Verify(x => x.AddUser(null), Times.Never);
        }

        [TestMethod]
        public void AddUser_ValidUser_ReturnsOkResult()
        {
            var controller = new UserController(UserServiceMock.Object);
            var dto = Mocker.CreateInstance<DTO.User>();
            UserServiceMock.Setup(x => x.AddUser(It.IsAny<User>())).Verifiable();

            var result = controller.AddUser(dto);
            Assert.IsTrue(result is OkResult);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void UpdateUser_NullUser_ReturnsBadRequestResult()
        {
            //var controller = new UserController(UserServiceMock.Object);
            //UserServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Verifiable();
            var controller = Mocker.CreateInstance<UserController>();
            var result = controller.UpdateUser(null);
            Assert.IsTrue(result is BadRequestResult);

            //UserServiceMock.Verify(x => x.UpdateUser(null), Times.Never);
        }

        [TestMethod]
        public void UpdateUser_ValidUser_ReturnsOkResult()
        {
            var controller = new UserController(UserServiceMock.Object);
            var dto = Mocker.CreateInstance<DTO.User>();
            UserServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Verifiable();

            var result = controller.UpdateUser(dto);
            Assert.IsTrue(result is OkResult);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void RemoveUser_NullUser_ReturnsBadRequestResult()
        {
            var controller = new UserController(UserServiceMock.Object);
            UserServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Verifiable();

            var result = controller.RemoveUser(null);
            Assert.IsTrue(result is BadRequestResult);

            UserServiceMock.Verify(x => x.RemoveUser(null), Times.Never);
        }

        [TestMethod]
        public void RemoveUser_ValidUser_ReturnsOkResult()
        {
            var controller = new UserController(UserServiceMock.Object);
            var dto = Mocker.CreateInstance<DTO.User>();
            UserServiceMock.Setup(x => x.RemoveUser(It.IsAny<User>())).Verifiable();

            var result = controller.RemoveUser(dto);
            Assert.IsTrue(result is OkResult);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void FetchAll_ReturnsListOfUsers()
        {
            var controller = new UserController(UserServiceMock.Object);
            //var userList = Mocker.CreateInstance<List<User>>();

            UserServiceMock.Setup(x => x.FetchAll())
                .Returns(new List<User>())
                .Verifiable();

            var result = controller.FetchAll();
            Assert.IsTrue(result.Value.GetType() == typeof(List<DTO.User>));
        }
    }
}
