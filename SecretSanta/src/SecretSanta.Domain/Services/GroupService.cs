using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentException("groupId <= 0 on call to GroupService.AddGroupUser(int groupId, User user).",
                    nameof(groupId));
            }

            if (user == null)
            {
                throw new ArgumentNullException("user was null on call to GroupService.AddGroupUser(int groupId, User user).",
                    nameof(user));
            }

            var group = DbContext.Groups.Single(g => g.Id == groupId);

            var groupUser = new GroupUser
            {
                GroupId = groupId,
                User = user,
                UserId = user.Id
            };

            group?.GroupUsers?.Add(groupUser);
            DbContext.SaveChanges();
            
            return user;
        }

        public void RemoveGroupUser(int groupId, User user)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("groupId <= 0 on call to GroupService.RemoveGroupUser(int groupId, User user).",
                    nameof(groupId));
            }

            if (user == null)
            {
                throw new ArgumentNullException("user was null on call to GroupService.RemoveGroupUser(int groupId, User user).",
                    nameof(user));
            }

            var group = DbContext.Groups.Single(g => g.Id == groupId);

            var groupUser = new GroupUser
            {
                GroupId = groupId,
                User = user,
                UserId = user.Id
            };

            group?.GroupUsers?.Remove(groupUser);
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
                throw new ArgumentException("groupId <= 0 on call to GroupService.FetchAllGroupUsers(int groupId).",
                    nameof(groupId));
            }

            return DbContext.Groups
                .Where(g => g.Id == groupId)?
                .SelectMany(g => g.GroupUsers)?
                .Select(g => g.User)
                .ToList();
        }
    }
}