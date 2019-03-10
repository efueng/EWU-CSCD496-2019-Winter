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
    public class GroupsPageTests
    {
        private const string RootUrl = "https://localhost:44334/";
        private IWebDriver Driver { get; set; }
        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Driver.Quit();
            //Driver.Dispose();
        }

        [TestMethod]
        public void CanGetToGroupsPage()
        {
            //Arrange
            Driver.Navigate().GoToUrl(RootUrl);


            //Act
            var homePage = new HomePage(Driver);
            homePage.GroupsLink.Click();

            //var uri = new Uri(Driver.Url);
            //uri.Host
            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddGroupsPage()
        {
            //Arrange
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, GroupsPage.Slug));
            var groupsPage = new GroupsPage(Driver);
            
            //Act
            groupsPage.AddGroupButton.Click();


            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(AddGroupPage.Slug));
        }

        [TestMethod]
        public void CanAddGroups()
        {
            //Arrange
            string groupName = "Group Name" + Guid.NewGuid().ToString();
            GroupsPage page = CreateGroup(groupName);

            //Act

            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(GroupsPage.Slug));
            List<string> groupNames = page.GroupNames;
            Assert.IsTrue(groupNames.Contains(groupName));
        }

        [TestMethod]
        public void CanDeleteGroup()
        {
            //Arrange
            string groupName = "Group Name" + Guid.NewGuid().ToString();
            GroupsPage page = CreateGroup(groupName);

            //Act



            //Assert
        }

        private GroupsPage CreateGroup(string groupName)
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, GroupsPage.Slug));

            var page = new GroupsPage(Driver);
            page.AddGroupButton.Click();

            var addGroupPage = new AddGroupPage(Driver);
            
            addGroupPage.GroupNameTextBox.SendKeys(groupName);
            addGroupPage.SubmitButton.Click();

            return page;
        }
    }
}
