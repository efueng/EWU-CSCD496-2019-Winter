using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.DTO
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Group() { }

        public Group(Domain.Models.Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Id = group.Id;
            Name = group.Name;
        }
    }
}
