using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.TestPages
{
    public class AddGroupPage
    {
        public const string Slug = GroupsPage.Slug + "/Add";
        public IWebDriver Driver { get; }
        public IWebElement GroupNameTextBox => Driver.FindElement(By.Id("Name"));
        public IWebElement SubmitButton =>
            Driver
            .FindElements(By.CssSelector("button.is-primary"))
            .Single(x => x.Text == "Submit");
        
        public AddGroupPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
