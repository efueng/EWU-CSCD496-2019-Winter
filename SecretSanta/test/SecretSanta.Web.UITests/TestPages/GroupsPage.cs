using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.TestPages
{
    public class GroupsPage
    {
        public const string Slug = "Groups";
        public IWebDriver Driver { get; }
        public IWebElement AddGroupButton => Driver.FindElement(By.LinkText("Add Group"));
        public IWebElement EditButton => Driver.FindElement(By.LinkText("Edit"));
        public IWebElement DeleteButton => Driver.FindElement(By.LinkText("Delete"));
        public AddGroupPage AddGroupPage => new AddGroupPage(Driver);
        public List<string> GroupNames
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
        public GroupsPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
