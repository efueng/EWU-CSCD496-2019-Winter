using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Domain.Models
{
    public class Tag : Entity
    {
        public string Name { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}
