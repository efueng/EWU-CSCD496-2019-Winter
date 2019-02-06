using AutoMapper;
using BlogEngine.Api.Controllers;
using BlogEngine.Api.Models;
using BlogEngine.Api.ViewModels;
using BlogEngine.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Api.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private CustomWebApplicationFactory<Startup> Factory { get; set; }

        [AssemblyInitialize]
        public static void ConfigureAutoMapper(TestContext context)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfileConfiguration()));
        }

        //[TestInitialize]
        //public void CreateWebFactory()
        //{
        public UsersControllerTests()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }
        //}

        [TestMethod]
        public async Task AddUserViaApi_FailsDueToMissingFirstName()
        {
            var client = Factory.CreateClient();

            var viewModel = new UserInputViewModel
            {
                FirstName = "",
                LastName = "Montoya"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/posts", stringContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            //var result = await response.Content.ReadAsStringAsync();

            //var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(result);

            //var errors = problemDetails.Extensions["errors"] as JObject;

            //var firstError = (JProperty)errors.First;

            //var somethingCool = firstError.Value[0];

            //Assert.AreEqual("The FirstName field is required.", ((JValue)somethingCool).Value);
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

            var stringContent = new StringContent(JsonConvert.SerializeObject(userViewModel), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/users", stringContent);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            //var result = await response.Content.ReadAsStringAsync();

            //var resultViewModel = JsonConvert.DeserializeObject<UserViewModel>(result);

            //Assert.AreEqual(userViewModel.FirstName, resultViewModel.FirstName);
        }

        [TestMethod]
        public void AddUser_AddedSuccessfully()
        {
            var testService = new TestableUserService();

            testService.AddUser_ToReturn = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya",
            };

            var mapper = Mapper.Instance;
            var controller = new UsersController(testService, mapper);

            var viewModel = new UserInputViewModel
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            var result = controller.Post(viewModel);

            Assert.IsTrue(result.Result is CreatedAtActionResult);
            Assert.AreEqual(viewModel.FirstName, testService.AddUser_UserAdded.FirstName);
            Assert.AreEqual(viewModel.LastName, testService.AddUser_UserAdded.LastName);
        }

        [TestMethod]
        public void AddUser_NullViewModel_ReturnsBadRequest()
        {
            var testService = new TestableUserService();

            var mapper = Mapper.Instance;
            var controller = new UsersController(testService, mapper);

            var result = controller.Post(null);

            Assert.IsTrue(result.Result is BadRequestResult);
        }
    }
}
