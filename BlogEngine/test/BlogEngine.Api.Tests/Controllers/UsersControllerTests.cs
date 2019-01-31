using BlogEngine.Api.Controllers;
using BlogEngine.Api.ViewModels;
using BlogEngine.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Api.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void AddUser_AddedSuccessfully()
        {
            var testService = new TestableUserService();

            testService.AddUser_ToReturn = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya",
            };

            var controller = new UsersController(testService);

            var viewModel = new UserInputViewModel
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            var result = controller.Post(viewModel);

            Assert.IsTrue(result.Result is OkObjectResult);
            Assert.AreEqual(viewModel.FirstName, testService.AddUser_UserAdded.FirstName);
            Assert.AreEqual(viewModel.LastName, testService.AddUser_UserAdded.LastName);
        }

        [TestMethod]
        public void AddUser_NullViewModel_ReturnsBadRequest()
        {
            var testService = new TestableUserService();

            var controller = new UsersController(testService);

            var result = controller.Post(null);

            Assert.IsTrue(result.Result is BadRequestResult);
        }
    }
}
