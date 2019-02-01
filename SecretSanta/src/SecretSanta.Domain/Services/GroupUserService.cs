using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GroupUserService
    {
        private ApplicationDbContext DbContext { get; }
        public GroupUserService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public GroupUser AddGroupUser(Group group, User user)
        {
            return null;
        }
    }
}
