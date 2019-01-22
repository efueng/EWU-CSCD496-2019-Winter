using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class User : Entity
    {
        public User(string firstName = "Edmond", string lastName = "Dantes")
        {
            FirstName = firstName;
            LastName = lastName;
            Gifts = new List<Gift>();
        }

        private string _FirstName;
        public string FirstName
        {
            get => _FirstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("FirstName cannot be null or empty.", nameof(value));
                }

                value = value.Trim();
                _FirstName = value;
            }
        }

        private string _LastName;
        public string LastName
        {
            get => _LastName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("LastName cannot be null or empty.", nameof(value));
                }

                value = value.Trim();
                _LastName = value;
            }
        }
        
        public ICollection<Gift> Gifts { get; set; }
        //public ICollection<Group> Groups { get; set; }
    }
}
