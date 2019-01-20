using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Group : Entity
    {
        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Title cannot be null or empty.", "value");
                }

                value = value.Trim();
                _Title = value;
            }
        }
        
        public ICollection<User> Users { get; set; }
    }
}

