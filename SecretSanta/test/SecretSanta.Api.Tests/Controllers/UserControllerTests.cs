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

        //[TestMethod]
        //public async Task AddUserViaApi_FailsDueToMissingFirstName()
        //{
        //    var client = Factory.CreateClient();

        //    var viewModel = new UserInputViewModel
        //    {
        //        FirstName = "",
        //        LastName = "Montoya"
        //    };

        //    var response = await client.PostAsJsonAsync("/api/user", viewModel);

        //    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        //    var result = await response.Content.ReadAsStringAsync();

        //    var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

        //    var errors = problemDetails.Extensions["errors"] as JObject;

        //    var firstError = (JProperty)errors.First;

        //    var errorMessage = firstError.Value[0];

        //    Assert.AreEqual("The FirstName field is required.", ((JValue)errorMessage).Value);
        //    //var client = Factory.CreateClient();
        //    //var userInput = new UserInputViewModel { FirstName = "", LastName = "Woods" };

        //    //var stringContent = new StringContent(JsonConvert.SerializeObject(userInput), Encoding.UTF8, "application/json");
        //    //var response = await client.PostAsync("/api/User", stringContent);

        //    //Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        //}

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

            //var result = await response.Content.ReadAsStringAsync();

            //var resultViewModel = JsonConvert.DeserializeObject<UserViewModel>(result);

            //Assert.AreEqual(userViewModel.FirstName, resultViewModel.FirstName);
        }

        [TestMethod]
        public void DeleteUser_ReturnsOk()
        {
            var service = new Mock<IUserService>();
            service.Setup(x => x.DeleteUser(1))
                .Returns(true)
                .Verifiable();
            var controller = new UserController(service.Object, Mapper.Instance);

            IActionResult result = controller.DeleteUser(1);

            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void DeleteUser_ReturnsNotFoundWhenTheUserFailsToDelete()
        {
            var service = new Mock<IUserService>();
            service.Setup(x => x.DeleteUser(2))
                .Returns(false)
                .Verifiable();
            var controller = new UserController(service.Object, Mapper.Instance);

            IActionResult result = controller.DeleteUser(2);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public void DeleteUser_ReturnsOkWhenUserIsDeleted()
        {
            var service = new Mock<IUserService>();
            service.Setup(x => x.DeleteUser(2))
                .Returns(true)
                .Verifiable();
            var controller = new UserController(service.Object, Mapper.Instance);

            IActionResult result = controller.DeleteUser(2);

            Assert.IsTrue(result is OkResult);
        }
    }
}