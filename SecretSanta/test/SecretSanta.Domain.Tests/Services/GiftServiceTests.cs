using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class GiftServiceTests : DatabaseServiceTests
    {
        [TestMethod]
        public async Task AddGift()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                User user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = await userService.AddUser(user).ConfigureAwait(false);

                Gift gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1,
                    UserId = user.Id
                };

                Gift persistedGift = await giftService.AddGift(gift).ConfigureAwait(false);

                Assert.AreNotEqual(0, persistedGift.Id);
            }
        }

        [TestMethod]
        public async Task FetchGift()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                User user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = await userService.AddUser(user).ConfigureAwait(false);

                Gift gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1,
                    UserId = user.Id
                };

                Gift persistedGift = await giftService.AddGift(gift).ConfigureAwait(false);

                Assert.AreNotEqual(0, persistedGift.Id);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                Gift retrievedGift = await giftService.GetGift(1).ConfigureAwait(false);

                Assert.AreEqual("Sword", retrievedGift.Title);
            }
        }

        [TestMethod]
        public async Task UpdateGift()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                User user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = await userService.AddUser(user).ConfigureAwait(false);

                Gift gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1,
                    UserId = user.Id
                };

                Gift persistedGift = await giftService.AddGift(gift).ConfigureAwait(false);

                Assert.AreNotEqual(0, persistedGift.Id);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                System.Collections.Generic.List<User> users = await userService.FetchAll().ConfigureAwait(false);
                System.Collections.Generic.List<Gift> gifts = await giftService.GetGiftsForUser(users[0].Id).ConfigureAwait(false);

                Assert.IsTrue(gifts.Count > 0);

                gifts[0].Title = "Horse";
                await giftService.UpdateGift(gifts[0]).ConfigureAwait(false);                
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                System.Collections.Generic.List<User> users = await userService.FetchAll().ConfigureAwait(false);
                System.Collections.Generic.List<Gift> gifts = await giftService.GetGiftsForUser(users[0].Id).ConfigureAwait(false);

                Assert.IsTrue(gifts.Count > 0);
                Assert.AreEqual("Horse", gifts[0].Title);            
            }
        }

        [TestMethod]
        public async Task DeleteGift()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                User user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };

                user = await userService.AddUser(user).ConfigureAwait(false);

                Gift gift = new Gift
                {
                    Title = "Sword",
                    OrderOfImportance = 1,
                    UserId = user.Id
                };

                Gift persistedGift = await giftService.AddGift(gift).ConfigureAwait(false);

                Assert.AreNotEqual(0, persistedGift.Id);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);

                await giftService.RemoveGift(1).ConfigureAwait(false);
            }

            using (ApplicationDbContext context = new ApplicationDbContext(Options))
            {
                GiftService giftService = new GiftService(context);
                UserService userService = new UserService(context);

                System.Collections.Generic.List<User> users = await userService.FetchAll().ConfigureAwait(false);
                System.Collections.Generic.List<Gift> gifts = await giftService.GetGiftsForUser(users[0].Id).ConfigureAwait(false);

                Assert.IsTrue(gifts.Count == 0);
            }
        }
    }
}