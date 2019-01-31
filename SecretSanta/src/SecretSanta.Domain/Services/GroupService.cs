using SecretSanta.Domain.Models;
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

        public User AddGroupUser(int groupId, User user)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("groupId <= 0 on call to GroupService.AddGroupUser(int groupId, User user).");
            }

            //DbContext.Groups.Select
            return null;
        }

        public List<Group> FetchAll()
        {
            return DbContext.Groups.ToList();
        }

        public List<User> GetUsers(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("groupId <= 0 on call to GroupService.FetchAllGroupUsers(int groupId).", nameof(groupId));
            }

            return DbContext.Groups
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.GroupUsers)
                .Select(g => g.User)
                .ToList();
        }

        public List<User> FetchAllGroupUsers(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("groupId <= 0 on call to GroupService.FetchAllGroupUsers(int groupId).", nameof(groupId));
            }

            return DbContext.Groups
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.GroupUsers)
                .Select(g => g.User)
                .ToList();
        }

        public List<User> FetchAllGroupUsers(Group group)
        {
            throw new NotImplementedException();
        }
    }
}