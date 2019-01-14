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
        public string FirstName
        {
            get => _FirstName;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("value");
                }

                value = value.Trim();

                if (value is "")
                {
                    throw new ArgumentException("FirstName cannot be blank.", "Value");
                }

                _FirstName = value;
            }
        }
        private string _FirstName;
        public string LastName
        {
            get => _LastName;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("value");
                }

                value = value.Trim();

                if (value is "")
                {
                    throw new ArgumentException("LastName cannot be empty.", "vaule");
                }

                _LastName = value;
            }
        }
        private string _LastName;
        public ICollection<Gift> Gifts { get; set; }
    }
}
