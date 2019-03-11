using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.TestPages
{
    public class UsersPage
    {
        public const string Slug = "Users";
        public IWebDriver Driver { get; }
        public IWebElement AddUserButton => Driver.FindElement(By.LinkText("Add User"));
        public IWebElement EditButton => Driver.FindElement(By.LinkText("Edit"));
        public IWebElement DeleteButton => Driver.FindElement(By.LinkText("Delete"));
        public AddUserPage AddUserPage => new AddUserPage(Driver);
        public List<string> UserNames
        {
            get
            {
                var elements = Driver.FindElements(By.CssSelector("h1 + ul > li"));
                return elements
                    .Select(e =>
                    {
                        var text = e.Text;
                        if (text.EndsWith(" Edit Delete"))
                        {
                            text = text.Substring(0, text.Length - " Edit Delete".Length);
                        }
                        return text;
                    })
                    .ToList();
            }
        }

        public IWebElement GetDeleteLink(string userName)
        {
            IReadOnlyCollection<IWebElement> deleteLinks =
                Driver.FindElements(By.CssSelector("a.is-danger"));

            return deleteLinks.Single(x => x.GetAttribute("onclick").EndsWith($"{userName}?')"));
        }

        public UsersPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
