using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using SecretSanta.Api.Models;
using Moq;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests
    {
        //private 
        [AssemblyInitialize]
        public static void ConfigureAutoMapper(TestContext context)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfileConfiguration()));
        }
        [TestMethod]
        public void GetGiftForUser_ReturnsUsersFromService()
        {
            var gift = new Gift
            {
                Id = 3,
                Title = "Gift Tile",
                Description = "Gift Description",
                Url = "http://www.gift.url",
                OrderOfImportance = 1,
                UserId = 1
            };
            //var testService = new TestableGiftService
            //{
            //    ToReturn = new List<Gift>
            //    {
            //        gift
            //    }
            //};

            var mapper = Mapper.Instance;

            var mockService = new Mock<IGiftService>(MockBehavior.Strict);
            mockService.Setup(x => x.GetGiftsForUser(1))
                .Returns(new List<Gift> { gift })
                .Verifiable();

            var controller = new GiftController(mockService.Object, mapper);

            IActionResult result = controller.GetGiftForUser(1);
            Assert.IsTrue(result is OkObjectResult);
            var okObjectResult = (OkObjectResult)result;
            var resultList = (List<GiftViewModel>)okObjectResult.Value;
            //Assert.AreEqual(4, testService.GetGiftsForUser_UserId);

            GiftViewModel resultGift = resultList.Single();
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
            Assert.AreEqual(gift.Description, resultGift.Description);
            Assert.AreEqual(gift.Url, resultGift.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);

            mockService.VerifyAll();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void GetGiftForUser_RequiresUserIdGreaterThanZero(int userId)
        {
            //var testService = new TestableGiftService();
            var mockService = new Mock<IGiftService>(MockBehavior.Strict);
            var controller = new GiftController(mockService.Object, Mapper.Instance);

            IActionResult result = controller.GetGiftForUser(userId);

            Assert.IsTrue(result is NotFoundResult);
            mockService.Verify(x => x.GetGiftsForUser(userId), Times.Never);
            mockService.VerifyAll();
            //This check ensures that the service was not called
            //Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }
    }
}
