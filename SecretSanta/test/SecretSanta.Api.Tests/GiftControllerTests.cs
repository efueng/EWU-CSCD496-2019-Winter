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
            //GiftControllerMock = Mocker.CreateInstance<GiftController>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftForUser_ReturnsUsersFromService()
        {
            var mocker = new AutoMocker();
            var giftMock = mocker.CreateInstance<Gift>();
            var controllerMock = mocker.CreateInstance<GiftController>();
            var result = controllerMock.GetGiftForUser(giftMock.UserId);

            Assert.IsTrue(result is ActionResult<List<DTO.Gift>>);
            Mocker.VerifyAll();
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var mocker = new AutoMocker();
            var giftMock = mocker.CreateInstance<Gift>();
            var giftServiceMock = mocker.GetMock<IGiftService>();
            var controllerMock = mocker.CreateInstance<GiftController>();

            //giftServiceMock.Setup(x => x.GetGiftsForUser(-1))
            //    .Returns(ActionResult)
            //    .Verifiable();

            //var notFound = Act
            var result = controllerMock.GetGiftForUser(-1);
            
            Assert.IsTrue(result.Result is NotFoundResult);
            mocker.VerifyAll();
            giftServiceMock.VerifyAll();
        }

        [TestMethod]
        public void AddGiftToUser_RequiresGift()
        {
            //var testService = new TestableGiftService();
            //var controller = new GiftController(testService);

            //ActionResult<DTO.Gift> result = controller.AddGiftToUser(4, null);

            //Assert.IsTrue(condition: result.GetType() == typeof(DTO.Gift));

            //// this check ensure that the service was not called
            //Assert.AreEqual(0, testService.AddGiftToUser_UserId);

            var controllerMock = Mocker.CreateInstance<GiftController>();
            var result = controllerMock.AddGiftToUser(4, null);
            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public void AddGiftToUser_InvokesService()
        {
            //var testService = new TestableGiftService();
            //var controller = new GiftController(testService);
            var controller = Mocker.CreateInstance<GiftController>();

            ActionResult result = controller.AddGiftToUser(4, new DTO.Gift());
            Assert.IsTrue(result is OkResult);
            Assert.IsNotNull(result, "Result was not a 200");
        }
    }
}
