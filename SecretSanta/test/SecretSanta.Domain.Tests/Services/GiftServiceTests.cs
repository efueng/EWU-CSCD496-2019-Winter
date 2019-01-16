using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class GiftServiceTests
    {
        Gift CreateNewGift()
        {
            var user = new User
            {
                FirstName = "Wedge",
                LastName = "Antilles"
            };

            var gift = new Gift
            {
                Title = "Blaster",
                OrderOfImportance = 1,
                Url = "www.pewpew.com",
                Description = "Designed on Corellia",
                User = user
            };

            return gift;
        }

        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<ApplicationDbContext> Options { get; set; }

        [TestInitialize]
        public void OpenConnection()
        {
            SqliteConnection = new SqliteConnection("DataSource=:memory:");
            SqliteConnection.Open();

            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(SqliteConnection)
                .EnableSensitiveDataLogging()
                .Options;

            using (var context = new ApplicationDbContext(Options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestCleanup]
        public void CloseConnection()
        {
            SqliteConnection.Close();
        }

        [TestMethod]
        public void AddGift()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GiftService service = new GiftService(context);
                var myGift = CreateNewGift();
                var persistedGift = service.AddGift(myGift);

                Assert.AreNotEqual(0, persistedGift.Id);
            }
        }
    }
}
