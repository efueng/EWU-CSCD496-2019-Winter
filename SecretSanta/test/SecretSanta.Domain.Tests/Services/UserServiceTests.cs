using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Tests.Models
{
    [TestClass]
    public class UserServiceTests
    {
        User CreateUser()
        {
            var user = new User
            {
                FirstName = "Edmond",
                LastName = "Dantes"
            };

            var gift = new Gift
            {
                Title = "Socks",
                Description = "Warm and fuzzy",
                OrderOfImportance = 1,
                Url = "www.socks.com",
                User = user
            };

            user.Gifts = new List<Gift>();
            user.Gifts.Add(gift);

            return user;
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
                //.UseLoggerFactory(GetLoggerFactory())
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
        public void AddUser()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                UserService service = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = service.AddUser(myUser);

                Assert.AreNotEqual(0, persistedUser.Id);
            }
        }

        [TestMethod]
        public void FindUser()
        {
            // arrange
            using (var context = new ApplicationDbContext(Options))
            {
                UserService service = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = service.AddUser(myUser);
            }

            // act
            using (var context = new ApplicationDbContext(Options))
            {
                UserService service = new UserService(context);
                var fetchedUser = service.Find(1);

                // assert
                Assert.AreEqual(1, fetchedUser.Gifts.Count);
            }
        }
    }
}
