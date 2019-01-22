using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Gift : Entity
    {
        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                value = value.Trim();

                if (value is "")
                {
                    throw new ArgumentException("Title cannot be empty.", nameof(value));
                }

                _Title = value;
            }
        }

        private int _OrderOfImportance;
        public int OrderOfImportance
        {
            get => _OrderOfImportance;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "OrderOfImportance must be at least 1.");
                }

                _OrderOfImportance = value;
            }
        }

        private string _Url;
        public string Url
        {
            get => _Url;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("URL cannot be null or empty.", nameof(value));
                }

                value = value.Trim();
                _Url = value;
            }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Description cannot be null or empty.", nameof(value));
                }

                value = value.Trim();
                _Description = value;
            }
        }
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
