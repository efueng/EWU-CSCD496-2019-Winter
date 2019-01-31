using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext DbContext { get; }

        public GroupService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Group AddGroup(Group @group)
        {
            DbContext.Groups.Add(@group);
            DbContext.SaveChanges();
            return @group;
        }

        public Group UpdateGroup(Group @group)
        {
            DbContext.Groups.Update(@group);
            DbContext.SaveChanges();
            return @group;
        }

        public void RemoveGroup(Group @group)
        {
            if (@group == null)
            {
                throw new ArgumentNullException(nameof(@group));
            }

            DbContext.Remove(@group);
            DbContext.SaveChanges();
        }

        public List<Group> FetchAll()
        {
            return DbContext.Groups.ToList();
        }

        public List<User> FetchAllGroupUsers(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("groupId must be greater than zero.", nameof(groupId));
            }

            return DbContext.Groups.Include(g => g.GroupUsers)
                .SingleOrDefault(g => g.Id == groupId)  // get group whose Id == groupId
                .GroupUsers                             // get GroupUsers of the group
                .Select(gu => gu.User).ToList();        // 
        }

        public List<User> FetchAllGroupUsers(Group @group)
        {
            if (@group == null)
            {
                throw new ArgumentNullException("@group parameter was null.", nameof(group));
            }

            return @group.GroupUsers.Select(gu => gu.User).ToList();
        }
    }
}