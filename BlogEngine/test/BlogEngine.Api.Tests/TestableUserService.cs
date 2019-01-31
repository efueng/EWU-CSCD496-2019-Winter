using BlogEngine.Domain.Models;
using BlogEngine.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Api.Tests
{
    public class TestableUserService : IUserService
    {
        public User AddUser_UserAdded {get;set;}
        public User AddUser_ToReturn { get; set; }

        public User AddUser(User user)
        {
            AddUser_UserAdded = user;
            return AddUser_ToReturn;
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public User Find(int id)
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
