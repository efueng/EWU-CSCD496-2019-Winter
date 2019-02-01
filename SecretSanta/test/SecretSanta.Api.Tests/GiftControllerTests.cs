using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Moq.AutoMock;
using SecretSanta.Domain.Services;
using Moq;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GiftControllerTests
    {
        private AutoMocker Mocker { get; set; }
        private Mock<IGiftService> GiftServiceMock { get; set; }
        private Mock<GiftController> GiftControllerMock { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Mocker = new AutoMocker();
            GiftServiceMock = Mocker.GetMock<IGiftService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftForUser_ReturnsGiftsFromService()
        {
            var mocker = new AutoMocker(MockBehavior.Strict);
            var service = mocker.GetMock<IGiftService>();
            var controller = new GiftController(service.Object);
            var giftMock = mocker.CreateInstance<Gift>();
            giftMock.UserId = 1;

            service.Setup(x => x.GetGiftsForUser(giftMock.UserId))
                .Returns(new List<Gift>())
                .Verifiable();

            // Act
            var result = controller.GetGiftForUser(giftMock.UserId);

            // Assert
            Assert.IsTrue(result.Value is List<DTO.Gift>);
            service.Verify(x => x.GetGiftsForUser(giftMock.UserId), Times.Once);
            mocker.VerifyAll();
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            // Arrange
            var mocker = new AutoMocker(MockBehavior.Strict);
            var service = mocker.GetMock<IGiftService>();
            var controller = new GiftController(service.Object);

            service.Setup(x => x.GetGiftsForUser(-1)).Verifiable();

            // Act
            var result = controller.GetGiftForUser(-1);

            // Assert
            Assert.IsTrue(result.Result is NotFoundResult);
            service.Verify(x => x.GetGiftsForUser(-1), Times.Never);
        }

        [TestMethod]
        public void GetGiftsForUser_ReturnsListOfGifts()
        {
            // Arrange
            var gift = Mock.Of<Gift>();
            gift.Id = 1;
            gift.Title = "Test Gift";
            gift.Description = "A fake gift.";
            gift.UserId = 42;
            gift.Url = "fake.net";
            gift.OrderOfImportance = 1;
            
            List<Gift> giftList = new List<Gift>();
            giftList.Add(gift);

            GiftServiceMock.Setup(x => x.GetGiftsForUser(1))
                .Returns(giftList)
                .Verifiable();
            
            var controllerMock = new GiftController(GiftServiceMock.Object);

            // Act
            var result = controllerMock.GetGiftForUser(1);
            var resultDTO = result.Value.Single();

            // Assert
            Assert.AreEqual(gift.Id, resultDTO.Id);
            Assert.AreEqual(gift.Title, resultDTO.Title);
            Assert.AreEqual(gift.Description, resultDTO.Description);
            Assert.AreEqual(gift.Url, resultDTO.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultDTO.OrderOfImportance);
            Mocker.VerifyAll();
            GiftServiceMock.VerifyAll();
        }

        [TestMethod]
        public void AddGiftToUser_RequiresGift()
        {
            var controllerMock = Mocker.CreateInstance<GiftController>();
            var result = controllerMock.AddGiftToUser(1, null);
            Assert.IsTrue(result is BadRequestResult);
            
        }

        [TestMethod]
        public void AddGiftToUser_NegativeUserId_ReturnsNotFound()
        {
            // Arrange
            var strictMocker = new AutoMocker(MockBehavior.Strict);
            var controller = strictMocker.CreateInstance<GiftController>();
            var service = Mocker.GetMock<IGiftService>();

            service.Setup(x => x.AddGiftToUser(-1, It.IsAny<Gift>())).Verifiable();

            // Act
            var result = controller.AddGiftToUser(-1, It.IsAny<DTO.Gift>());

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            service.Verify(x => x.AddGiftToUser(-1, It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void AddGiftToUser_InvokesService()
        {
            var controller = Mocker.CreateInstance<GiftController>();

            ActionResult result = controller.AddGiftToUser(4, new DTO.Gift());
            Assert.IsTrue(result is OkResult);
            Assert.IsNotNull(result, "Result was not a 200");
        }

        [TestMethod]
        public void RemoveGiftForUser()
        {
            // Arrange
            var gift = Mocker.CreateInstance<DTO.Gift>();
            var controllerMock = new GiftController(GiftServiceMock.Object);
            GiftServiceMock.Setup(x => x.RemoveGift(It.IsAny<Gift>()))
                .Verifiable();

            // Act
            var result = controllerMock.RemoveGiftForUser(gift);

            // Assert
            Assert.IsTrue(result is OkResult);

            // Check ensure service was invoked only once. Is this necessary?
            GiftServiceMock.Verify(x => x.RemoveGift(It.IsAny<Gift>()), Times.Once);
        }

        [TestMethod]
        public void RemoveGiftForUser_NegativeUserId_ReturnsNotFound()
        {
            // Arrange
            var gift = Mocker.CreateInstance<DTO.Gift>();
            var controllerMock = new GiftController(GiftServiceMock.Object);
            GiftServiceMock.Setup(x => x.RemoveGift(It.IsAny<Gift>()))
                .Verifiable();

            // Act
            var result = controllerMock.RemoveGiftForUser(null);

            // Assert
            Assert.IsTrue(result is BadRequestResult);

            // This check ensures the service was not called
            GiftServiceMock.Verify(x => x.RemoveGift(It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void UpdateGiftForUser_RequiresPositiveUserId()
        {
            // Arrange
            var mocker = new AutoMocker(MockBehavior.Strict);
            var controller = mocker.CreateInstance<GiftController>();
            var service = Mocker.GetMock<IGiftService>();

            service.Setup(x => x.UpdateGiftForUser(-1, It.IsAny<Gift>())).Verifiable();

            // Act
            var result = controller.UpdateGiftForUser(-1, It.IsAny<DTO.Gift>());

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            mocker.VerifyAll();
            service.Verify(x => x.UpdateGiftForUser(-1, It.IsAny<Gift>()), Times.Never);
        }

        [TestMethod]
        public void UpdateGiftForUser_ReturnsOkResult()
        {
            var mocker = new AutoMocker(MockBehavior.Strict);
            var controller = mocker.CreateInstance<GiftController>();
            var service = mocker.GetMock<IGiftService>();
            var dto = mocker.CreateInstance<DTO.Gift>();

            service.Setup(x => x.UpdateGiftForUser(1, It.IsAny<Gift>()))
                .Returns(new Gift()) // this is wrong and temporary magic. This just makes things work for some reason
                .Verifiable();
            
            var result = controller.UpdateGiftForUser(1, dto);

            Assert.IsTrue(result is OkResult);
            mocker.VerifyAll();
            service.Verify(x => x.UpdateGiftForUser(1, It.IsAny<Gift>()), Times.Once);
        }
    }
}
