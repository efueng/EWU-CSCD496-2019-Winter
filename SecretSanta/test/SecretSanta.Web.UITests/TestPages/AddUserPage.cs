using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.TestPages
{
    public class AddUserPage
    {
        public const string Slug = UsersPage.Slug + "/Add";
        public IWebDriver Driver { get; }
        public IWebElement FirstNameTextBox => Driver.FindElement(By.Id("FirstName"));
        public IWebElement LastNameTextBox => Driver.FindElement(By.Id("LastName"));
        public IWebElement SubmitButton =>
            Driver
            .FindElements(By.CssSelector("button.is-primary"))
            .Single(x => x.Text == "Submit");

        public AddUserPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
