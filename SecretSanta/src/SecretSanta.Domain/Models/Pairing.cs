using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Pairing : Entity
    {
        public User Recipient { get; set; }
        public User Santa { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}
