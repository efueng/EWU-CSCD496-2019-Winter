using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.UITests.TestPages;
using System;
using System.Collections.Generic;
using System.IO;
//using Microsoft.Extensions.Configuration;

namespace SecretSanta.Web.UITests
{
    [TestClass]
    public class UsersPageTests
    {
        private const string RootUrl = "https://localhost:44334/";
        private IWebDriver Driver { get; set; }
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                string projectRoot =
                    Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("bin"));

                string fileName = $"{projectRoot}/Screenshots/{TestContext.TestName}.png";

                Screenshot screenshot = ((ITakesScreenshot) Driver).GetScreenshot();
                screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
            }

            Driver.Quit();
            Driver.Dispose();
        }

        [TestMethod]
        public void CanGetToUsersPage()
        {
            //Arrange
            Driver.Navigate().GoToUrl(RootUrl);


            //Act
            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();

            //var uri = new Uri(Driver.Url);
            //uri.Host
            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddUsersPage()
        {
            //Arrange
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            var usersPage = new UsersPage(Driver);

            //Act
            usersPage.AddUserButton.Click();


            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }

        [TestMethod]
        public void CanAddUsers()
        {
            //Arrange
            string firstName = "FirstName";
            string lastName = "LastName" + Guid.NewGuid().ToString();
            UsersPage page = CreateUser(firstName, lastName);

            //Act

            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
            List<string> userNames = page.UserNames;
            Assert.IsTrue(userNames.Contains($"{firstName} {lastName}"));
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            //Arrange
            string firstName = "FirstName";
            string lastName = "LastName" + Guid.NewGuid().ToString();
            UsersPage page = CreateUser(firstName, lastName);

            //Act
            var deleteLink = page.GetDeleteLink($"{firstName} {lastName}");
            deleteLink.Click();
            Driver.SwitchTo().Alert().Accept();

            //Assert
            List<string> userNames = page.UserNames;
            Assert.IsFalse(userNames.Contains($"{firstName} {lastName}"));
        }

        [TestMethod]
        public void FailToDeleteUserTakesScreenshot()
        {
            //Arrange
            string firstName = "FirstName";
            string lastName = "LastName" + Guid.NewGuid().ToString();
            UsersPage page = CreateUser(firstName, lastName);

            //Act
            var deleteLink = page.GetDeleteLink($"{firstName} {lastName}");
            deleteLink.Click();
            Driver.SwitchTo().Alert().Accept();
            //Assert
            List<string> userNames = page.UserNames;
            Assert.IsTrue(userNames.Contains($"{firstName} {lastName}"));
        }

        private UsersPage CreateUser(string firstName, string lastName)
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));

            var page = new UsersPage(Driver);
            page.AddUserButton.Click();

            var addUserPage = new AddUserPage(Driver);

            addUserPage.FirstNameTextBox.SendKeys(firstName);
            addUserPage.LastNameTextBox.SendKeys(lastName);
            addUserPage.SubmitButton.Click();

            return page;
        }
    }
}
