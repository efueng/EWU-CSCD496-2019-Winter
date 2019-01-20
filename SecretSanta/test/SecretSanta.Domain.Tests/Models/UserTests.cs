using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateUser()
        {
            User user = new User { FirstName = "Edmond", LastName = "Dantes" };
            Assert.AreEqual("Edmond", user.FirstName);
            Assert.AreEqual("Dantes", user.LastName);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(default(string))]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void FirstName_AssignedNullOrEmpty_ThrowsArgumentException(string value)
        {
            User sut = new User(value, value);
        }
    }
}
