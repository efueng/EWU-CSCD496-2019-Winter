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
    public class PairingServiceTests
    {
        Pairing CreatePairing()
        {
            User santa = new User
            {
                FirstName = "Edmond",
                LastName = "Dantes"
            };

            User recipient = new User
            {
                FirstName = "Fernand",
                LastName = "Mondego"
            };

            Group group = new Group { Title = "Frienemies" };

            Pairing pairing = new Pairing
            {
                Santa = santa,
                Recipient = recipient,
                Group = group
            };

            return pairing;
        }

        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<ApplicationDbContext> Options { get; set; }
        public object PairinService { get; private set; }

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
        public void AddPairing()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService service = new PairingService(context);
                var pairing = CreatePairing();
                var persistedPairing = service.AddPairing(pairing);

                Assert.AreNotEqual(0, persistedPairing.Id);
                Assert.AreNotEqual(0, persistedPairing.GroupId);
                Assert.AreNotEqual(0, persistedPairing.Santa.Id);
                Assert.AreNotEqual(0, persistedPairing.Recipient.Id);
            }
        }

        [TestMethod]
        public void FindPairing()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService service = new PairingService(context);
                var pairing = CreatePairing();
                var persistedPairing = service.AddPairing(pairing);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                PairingService service = new PairingService(context);
                var fetchedPairing = service.Find(1);

                Assert.AreEqual("Edmond", fetchedPairing.Santa.FirstName);

            }
        }
    }
}
