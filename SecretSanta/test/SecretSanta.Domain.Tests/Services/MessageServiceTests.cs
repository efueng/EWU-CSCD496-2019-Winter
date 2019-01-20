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
    public class MessageServiceTests
    {
        Message CreateMessage()
        {
            var recipient = new User
            {
                FirstName = "Fernand",
                LastName = "Mondego"
            };

            var santa = new User
            {
                FirstName = "Edmond",
                LastName = "Dantes"
            };

            var message = new Message
            {
                Recipient = recipient,
                Santa = santa,
                Body = "For all evils there are two remedies - time and silence."
            };

            return message;
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
        public void AddMessage()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                MessageService service = new MessageService(context);
                var message = CreateMessage();
                var persistedMessage = service.AddMessage(message);

                Assert.AreNotEqual(0, persistedMessage.Id);
            }
        }

        [TestMethod]
        public void FindMessage()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                MessageService service = new MessageService(context);
                var message = CreateMessage();
                var persistedMessage = service.AddMessage(message);
                var fetchedMessage = service.Find(1);

                Assert.AreEqual(persistedMessage, fetchedMessage);
            }
        }
    }
}
