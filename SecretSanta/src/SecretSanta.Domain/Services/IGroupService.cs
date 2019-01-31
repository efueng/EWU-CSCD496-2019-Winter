using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {
        Group AddGroup(Group group);
        Group UpdateGroup(Group group);
        void RemoveGroup(Group group);
        List<User> FetchAllGroupUsers(int groupId);
        List<User> FetchAllGroupUsers(Group group);
        List<Group> FetchAll();
    }
}
