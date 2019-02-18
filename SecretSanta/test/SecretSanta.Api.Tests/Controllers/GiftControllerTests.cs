using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.Models;
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
    public class GiftControllerTests
    {
        [AssemblyInitialize]
        public static void ConfigureAutoMapper(TestContext context)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfileConfiguration()));
        }

        [TestMethod]
        public async Task GetGiftForUser_ReturnsUsersFromService()
        {
            var gift = new Gift
            {
                Id = 3,
                Title = "Gift Tile",
                Description = "Gift Description",
                Url = "http://www.gift.url",
                OrderOfImportance = 1
            };
            var testService = new TestableGiftService
            {
                ToReturn = new List<Gift>
                {
                    gift
                }
            };
            var controller = new GiftsController(testService, Mapper.Instance);

            var result = await controller.GetGiftsForUser(4) as OkObjectResult;

            Assert.AreEqual(4, testService.GetGiftsForUser_UserId);
            GiftViewModel resultGift = ((List<GiftViewModel>)result.Value).Single();
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
            Assert.AreEqual(gift.Description, resultGift.Description);
            Assert.AreEqual(gift.Url, resultGift.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);
        }

        [TestMethod]
        public async Task GetGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftsController(testService, Mapper.Instance);

            var result = await controller.GetGiftsForUser(-1) as NotFoundResult;

            Assert.IsNotNull(result);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }

        [TestMethod]
        [DataRow(-1, null)]
        [DataRow(0, null)]
        [DataRow(1, null)]
        public async Task UpdateGiftViaApi_InvalidParameters_ReturnsBadRequest(int userId, GiftViewModel viewModel)
        {
            // Arrange
            var service = new Mock<IGiftService>(MockBehavior.Strict);
            var controller = new GiftsController(service.Object, Mapper.Instance);

            // Act
            var result = await controller.UpdateGiftForUser(userId, viewModel);

            // Assert
            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public async Task UpdateGiftViaApi_ValidParameters_ReturnsUpdatedGift()
        {
            // Arrange
            var gift = new GiftViewModel
            {
                Description = "Xbox",
            };

            var updatedGift = new GiftViewModel
            {
                Description = "Updated"
            };

            var service = new Mock<IGiftService>();// MockBehavior.Strict);
            service.Setup(x => x.UpdateGiftForUser(It.IsAny<int>(), It.IsAny<Gift>()))
                .ReturnsAsync(new Gift { Description = "Updated" })
                .Verifiable();

            var controller = new GiftsController(service.Object, Mapper.Instance);

            // Act
            IActionResult result = await controller.UpdateGiftForUser(1, updatedGift) as NoContentResult;
            
            Assert.IsNotNull(result);
            service.VerifyAll();
        }

        [TestMethod]
        public async Task DeleteGiftViaApi_RequiresValidGift()
        {
            var service = new Mock<IGiftService>(MockBehavior.Strict);
            var controller = new GiftsController(service.Object, Mapper.Instance);

            IActionResult result = await controller.DeleteGift(null);

            Assert.IsTrue(result is BadRequestResult);
        }
    }
}
