using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.Models;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        public UserControllerTests()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }
        [TestMethod]
        public void AddUser_FailsDueToMissingFirstName()
        {
            var mockService = new Mock<IUserService>(MockBehavior.Strict);
            mockService.Setup(x => x.AddUser(It.IsAny<User>()))
                .Returns(new User())
                .Verifiable();
            var controller = new UserController(mockService.Object, Mapper.Instance);

            var viewModel = new UserInputViewModel
            {
                FirstName = "",
                LastName = "Montoya"
            };
            
            IActionResult result = controller.PostAddUser(viewModel);
            Assert.AreEqual(HttpStatusCode.BadRequest, result);
            Mock.VerifyAll(mockService);
            mockService.Verify(x => x.AddUser(Mapper.Map<User>(viewModel)), Times.Never);
        }

        [TestMethod]
        public async Task AddUserViaApi_FailsDueToMissingFirstName()
        {
            //var client = Factory.CreateClient();

            //var viewModel = new UserInputViewModel
            //{
            //    FirstName = "",
            //    LastName = "Montoya"
            //};

            //var response = await client.PostAsJsonAsync("/api/user", viewModel);

            //Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            //var result = await response.Content.ReadAsStringAsync();

            //var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

            //var errors = problemDetails.Extensions["errors"] as JObject;

            //var firstError = (JProperty)errors.First;

            //var errorMessage = firstError.Value[0];

            //Assert.AreEqual("The FirstName field is required.", ((JValue)errorMessage).Value);
            var client = Factory.CreateClient();
            var userInut = new UserInputViewModel { FirstName = "", LastName = "Woods" };

            var stringContent = new StringContent(JsonConvert.SerializeObject(userInut), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/User", stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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

            var response = await client.PostAsJsonAsync("/api/User", userViewModel);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var resultViewModel = JsonConvert.DeserializeObject<UserViewModel>(result);

            Assert.AreEqual(userViewModel.FirstName, resultViewModel.FirstName);
        }
    }
}