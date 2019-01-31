using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Domain.Models
{
    public class Comment : Entity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Post Post { get; set; }
    }
}
