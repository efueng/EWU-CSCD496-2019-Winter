using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Group : Entity
    {
        public Group(string title)
        {
            Title = title;
            Users = new List<User>();
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("value");
                }

                value = value.Trim();

                if (value is "")
                {
                    throw new ArgumentException("Title cannot be empty.", "value");
                }

                _Title = value;
            }
        }
        
        public ICollection<User> Users { get; set; }

        public void AddUser(string firstName, string lastName)
        {
            User user = new User(firstName, lastName);
            Users.Add(user);
        }

        public void RemoveUser(string firstName, string lastName)
        {
            foreach(User u in Users)
            {
                if (u.FirstName.Equals(firstName) && u.LastName.Equals(lastName))
                {
                    Users.Remove(u);
                }
            }
        }

    }
}
