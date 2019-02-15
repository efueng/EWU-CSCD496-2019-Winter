using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.Services.Interfaces;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupUserControllerTests
    {
        [TestMethod]
        public async Task GetAllGroupUsers_ReturnsGroupUsers()
        {
            var GroupUser1 = new GroupUser
            {
                UserId = 1,
                Name = "GroupUser 1",
                G
            };
            var GroupUser2 = new GroupUser
            {
                Id = 2,
                Name = "GroupUser 2"
            };

            var service = new Mock<IGroupUserService>();
            service.Setup(x => x.FetchAll())
                .ReturnsAsync(new List<GroupUser> { GroupUser1, GroupUser2 })
                .Verifiable();


            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            var result = await controller.Get() as OkObjectResult;

            List<GroupUserViewModel> GroupUsers = ((IEnumerable<GroupUserViewModel>)result.Value).ToList();

            Assert.AreEqual(2, GroupUsers.Count);
            AssertAreEqual(GroupUsers[0], GroupUser1);
            AssertAreEqual(GroupUsers[1], GroupUser2);
            service.VerifyAll();
        }

        [TestMethod]
        public async Task CreateGroupUser_RequiresGroupUser()
        {
            var service = new Mock<IGroupUserService>(MockBehavior.Strict);
            var controller = new GroupUsersController(service.Object, Mapper.Instance);


            var result = await controller.Post(null) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateGroupUser_ReturnsCreatedGroupUser()
        {
            var GroupUser = new GroupUserInputViewModel
            {
                Name = "GroupUser"
            };
            var service = new Mock<IGroupUserService>();
            service.Setup(x => x.AddGroupUser(It.Is<GroupUser>(g => g.Name == GroupUser.Name)))
                .ReturnsAsync(new GroupUser
                {
                    Id = 2,
                    Name = GroupUser.Name
                })
                .Verifiable();

            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            var result = await controller.Post(GroupUser) as CreatedAtActionResult;
            var resultValue = result.Value as GroupUserViewModel;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(2, resultValue.Id);
            Assert.AreEqual("GroupUser", resultValue.Name);
            service.VerifyAll();
        }

        [TestMethod]
        public async Task UpdateGroupUser_RequiresGroupUser()
        {
            var service = new Mock<IGroupUserService>(MockBehavior.Strict);
            var controller = new GroupUsersController(service.Object, Mapper.Instance);


            IActionResult result = await controller.Put(1, null) as BadRequestResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateGroupUser_ReturnsUpdatedGroupUser()
        {
            var GroupUser = new GroupUserInputViewModel
            {
                Name = "GroupUser"
            };
            var service = new Mock<IGroupUserService>();
            service.Setup(x => x.GetById(2))
                .ReturnsAsync(new GroupUser
                {
                    Id = 2,
                    Name = GroupUser.Name
                })
                .Verifiable();

            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            IActionResult result = await controller.Put(2, GroupUser) as NoContentResult;

            Assert.IsNotNull(result);
            service.VerifyAll();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task DeleteGroupUser_RequiresPositiveId(int GroupUserId)
        {
            var service = new Mock<IGroupUserService>(MockBehavior.Strict);
            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            IActionResult result = await controller.Delete(GroupUserId);

            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task DeleteGroupUser_ReturnsNotFoundWhenTheGroupUserFailsToDelete()
        {
            var service = new Mock<IGroupUserService>();
            service.Setup(x => x.DeleteGroupUser(2))
                .ReturnsAsync(false)
                .Verifiable();
            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            IActionResult result = await controller.Delete(2);

            Assert.IsTrue(result is NotFoundResult);
            service.VerifyAll();
        }

        [TestMethod]
        public async Task DeleteGroupUser_ReturnsOkWhenGroupUserIsDeleted()
        {
            var service = new Mock<IGroupUserService>();
            service.Setup(x => x.DeleteGroupUser(2))
                .ReturnsAsync(true)
                .Verifiable();
            var controller = new GroupUsersController(service.Object, Mapper.Instance);

            IActionResult result = await controller.Delete(2);

            Assert.IsTrue(result is OkResult);
            service.VerifyAll();
        }

        private static void AssertAreEqual(GroupUserViewModel expected, GroupUser actual)
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null) Assert.Fail();

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}
