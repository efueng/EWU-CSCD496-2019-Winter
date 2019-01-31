using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    class GroupControllerTests
    {
        private AutoMocker Mocker { get; set; }
        private Mock<IGroupService> GroupServiceMock { get; set; }
        private Mock<GroupController> GroupControllerMock { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Mocker = new AutoMocker();
            GroupServiceMock = Mocker.GetMock<IGroupService>();
            GroupControllerMock = Mocker.GetMock<GroupController>();
            //UserControllerMock = new UserController(UserServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupController_RequiresUserService()
        {
            new GroupController(null);
        }

        [TestMethod]
        public void AddGroup_NullGroup_ReturnsBadRequestResult()
        {
            GroupControllerMock.Object.AddGroup(null);
        }
    }
}
