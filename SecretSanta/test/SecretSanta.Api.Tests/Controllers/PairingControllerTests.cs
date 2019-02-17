﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class PairingControllerTests
    {
        private CustomWebApplicationFactory<Startup> Factory { get; set; }
        private List<Pairing> Pairings { get; set; }

        [TestInitialize]
        public void CreateWebFactory()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        private Task<List<Pairing>> GetPairings(List<int> userIds, int groupId)
        {
            var pairings = new List<Pairing>();

            for (int idx = 0; idx < userIds.Count - 1; idx++)
            {
                var pairing = new Pairing
                {
                    SantaId = userIds[idx],
                    RecipientId = userIds[idx + 1],
                    OriginGroupId = groupId
                };

                pairings.Add(pairing);
            }

            var lastPairing = new Pairing
            {
                SantaId = userIds.Last(),
                RecipientId = userIds.First(),
                OriginGroupId = groupId
            };

            pairings.Add(lastPairing);

            return Task.FromResult(pairings);
        }

        [TestMethod]
        public async Task PostPairing_ValidGroupNumber_ReturnsCreated()
        {
            List<int> userIds = Enumerable.Range(1, 5).ToList();
            int groupId = 1;

            Pairings = await GetPairings(userIds, groupId);

            //MockPairingService.Setup(x => x.GeneratePairings(groupId)).Returns(pairings);
            //SetUpMockMapper();

            var service = new Mock<IPairingService>(MockBehavior.Strict);
            service.Setup(x => x.GeneratePairings(groupId))
                .ReturnsAsync(Pairings)
                .Verifiable();

            var controller = new PairingsController(service.Object, Mapper.Instance);

            var result = await controller.Post(groupId) as CreatedResult;
            var resultPairings = result.Value as List<PairingViewModel>;
            var firstPairing = resultPairings.First();
            var lastPairing = resultPairings.Last();

            Assert.AreEqual<int>(groupId, firstPairing.OriginGroupId);
            Assert.AreEqual<int>(1, firstPairing.SantaId);
            Assert.AreEqual<int>(2, firstPairing.RecipientId);
            Assert.AreEqual<int>(5, lastPairing.SantaId);
            Assert.AreEqual<int>(1, lastPairing.RecipientId);
            service.VerifyAll();
        }
    }
}