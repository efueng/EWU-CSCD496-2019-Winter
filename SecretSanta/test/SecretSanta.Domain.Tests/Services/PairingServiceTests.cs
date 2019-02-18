using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {

        [TestInitialize]
        public async Task TestInitialize()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GroupService groupService = new GroupService(context);
                UserService userService = new UserService(context);

                User user1 = new User
                {
                    Id = 42,
                    FirstName = "Edmond",
                    LastName = "Dantes"
                };

                User user2 = new User
                {
                    Id = 9938,
                    FirstName = "Fernand",
                    LastName = "Mondego"
                };

                User user3 = new User
                {
                    Id = 1,
                    FirstName = "Wedge",
                    LastName = "Antilles"
                };

                User user4 = new User
                {
                    Id = 99,
                    FirstName = "Boba",
                    LastName = "Fett"
                };

                Group group1 = new Group
                {
                    Name = "Group1"
                };

                Group group2 = new Group
                {
                    Name = "Group2"
                };

                await userService.AddUser(user1);
                await userService.AddUser(user2);
                await userService.AddUser(user3);
                await userService.AddUser(user4);

                await groupService.AddGroup(group1);
                await groupService.AddGroup(group2);

                await groupService.AddUserToGroup(group1.Id, user1.Id);
                await groupService.AddUserToGroup(group1.Id, user2.Id);
                await groupService.AddUserToGroup(group1.Id, user3.Id);
                await groupService.AddUserToGroup(group1.Id, user4.Id);

                await groupService.AddUserToGroup(group2.Id, user1.Id);
                await groupService.AddUserToGroup(group2.Id, user2.Id);
                await groupService.AddUserToGroup(group2.Id, user3.Id);
                await groupService.AddUserToGroup(group2.Id, user4.Id);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task GeneratPairings_RequiresPositiveGroupId(int groupId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => 
                    await pairingService.GeneratePairings(groupId));
            }
        }


        [TestMethod]
        public async Task GeneratePairings_GeneratesUniquePairings()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> pairings = await pairingService.GeneratePairings(1);
                
                var santaIds = pairings.Select(x => x.SantaId).ToList();
                var recipientIds = pairings.Select(x => x.RecipientId).ToList();

                Assert.AreEqual(4, santaIds.Distinct().Count());
                Assert.AreEqual(4, recipientIds.Distinct().Count());
            }
        }

        [TestMethod]
        public async Task GeneratePairings_ContainNoSelfGifters()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> pairings = await pairingService.GeneratePairings(1);

                foreach (var p in pairings)
                {
                    Assert.AreNotEqual<int>(p.SantaId, p.RecipientId);
                }
            }
        }

        [TestMethod]
        public async Task GeneratePairings_AddPairingsThatDifferOnlyByGroup()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> pairings1 = await pairingService.GeneratePairings(1);
                List<Pairing> pairings2 = await pairingService.GeneratePairings(2);

                List<int> pairing1UserIds = pairings1.Select(x => x.SantaId).ToList();
                List<int> pairing2UserIds = pairings2.Select(x => x.SantaId).ToList();

                Assert.AreEqual(pairings1.Select(x => x.OriginGroupId).First(), 1);
                Assert.AreEqual(pairings2.Select(x => x.OriginGroupId).First(), 2);

                foreach (int i in pairing1UserIds)
                {
                    Assert.IsTrue(pairing2UserIds.Contains(i));
                }
            }
        }
    }
}