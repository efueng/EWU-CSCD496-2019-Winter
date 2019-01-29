using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.DTO
{
    public class GroupUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public GroupUser() { }
        public GroupUser(Domain.Models.GroupUser groupUser)
        {
            if (groupUser == null)
            {
                throw new ArgumentNullException(nameof(groupUser));
            }

            GroupId = groupUser.GroupId;
            UserId = groupUser.UserId;
        }
    }
}
