using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Web.UITests.TestPages
{
    public class HomePage
    {
        public IWebDriver Driver { get; }
        public GroupsPage GroupsPage => new GroupsPage(Driver);

        public IWebElement GroupsLink => Driver.FindElement(By.CssSelector("a[href=\"/Groups\"]"));
        //public IWebElement GroupsLink => Driver.FindElement(By.LinkText("Groups"));

        public HomePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
