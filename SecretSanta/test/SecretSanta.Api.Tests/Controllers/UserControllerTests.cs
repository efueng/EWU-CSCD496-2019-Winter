using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private CustomWebApplicationFactory<Startup> Factory { get; set; }

        [TestInitialize]
        public void CreateWebFactory()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task AddUserViaApi_FailsDueToMissingFirstName()
        {
            var client = Factory.CreateClient();

            var viewModel = new UserInputViewModel
            {
                FirstName = "",
                LastName = "Montoya"
            };

            var response = await client.PostAsJsonAsync("/api/users", viewModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

            var errors = problemDetails.Extensions["errors"] as JObject;

            var firstError = (JProperty)errors.First;

            var errorMessage = firstError.Value[0];

            Assert.AreEqual("The FirstName field is required.", ((JValue)errorMessage).Value);
        }

        [TestMethod]
        public async Task AddUserViaApi_CompletesSuccessfully()
        {
            var client = Factory.CreateClient();

            var userViewModel = new UserInputViewModel
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            var response = await client.PostAsJsonAsync("/api/users", userViewModel);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var resultViewModel = JsonConvert.DeserializeObject<UserViewModel>(result);

            Assert.AreEqual(userViewModel.FirstName, resultViewModel.FirstName);
        }

        [TestMethod]
        public async Task GetAllUsersViaApi_CompletesSuccessfully()
        {
            // Arrange
            var user1 = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            var user2 = new User
            {
                FirstName = "Edmond",
                LastName = "Dantes"
            };

            var service = new Mock<IUserService>(MockBehavior.Strict);

            service.Setup(x => x.FetchAll())
                .ReturnsAsync(new List<User> { user1, user2 })
                .Verifiable();

            var controller = new UsersController(service.Object, Mapper.Instance);

            // Act
            var result = await controller.Get() as OkObjectResult;

            List<UserViewModel> users = ((IEnumerable<UserViewModel>)result.Value).ToList();

            // Assert
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(users[0].FirstName, "Inigo");
            Assert.AreEqual(users[1].FirstName, "Edmond");
            service.VerifyAll();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task GetUserByIdViaApi_RequiresPositiveId_ReturnsBadRequestResult(int userId)
        {
            // Arrange
            var service = new Mock<IUserService>(MockBehavior.Strict);
            //service.Setup(x => x.GetById(userId))
            //    .ReturnsAsync(new User())
            //    .Verifiable();

            var controller = new UsersController(service.Object, Mapper.Instance);

            // Act
            IActionResult result = await controller.Get(userId);

            // Assert
            Assert.IsTrue(result is BadRequestObjectResult);
            service.VerifyAll();
        }

        //[TestMethod]
        //[DataRow(1)]
        //[DataRow(2)]
        //public async Task GetUserByIdViaApi_UserNotFound_ReturnsNotFoundResult(int userId)
        //{
        //    // Arrange
        //    var service = new Mock<IUserService>(MockBehavior.Strict);
        //    //service.Setup(x => x.GetById(It.IsAny<int>()))
        //    //    .ReturnsAsync(Task.FromResult())
        //    //    .Verifiable();
        //    var controller = new UsersController(service.Object, Mapper.Instance);

        //    // Act
        //    IActionResult result = await controller.Get(userId);

        //    // Assert
        //    Assert.IsTrue(result is NotFoundResult);
        //    service.VerifyAll();
        //}

        [TestMethod]
        public async Task GetUserByIdViaApi_CompletesSuccessfully()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Edmond",
                LastName = "Dantes"
            };

            var service = new Mock<IUserService>(MockBehavior.Strict);
            service.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(new User { FirstName = "Edmond", LastName = "Dantes"})
                .Verifiable();

            var controller = new UsersController(service.Object, Mapper.Instance);

            // Act
            var result = await controller.Get(1) as OkObjectResult;
            UserViewModel viewModel = ((UserViewModel)result.Value);

            // Assert
            Assert.AreEqual("Edmond", viewModel.FirstName);
            Assert.AreEqual("Dantes", viewModel.LastName);
            service.VerifyAll();
        }
    }
}
