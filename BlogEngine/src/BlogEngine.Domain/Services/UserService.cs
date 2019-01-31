using BlogEngine.Domain.Models;
using BlogEngine.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogEngine.Domain.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext DbContext { get; set; }
        public UserService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public User AddUser(User user)
        {
            DbContext.Users.Add(user);

            DbContext.SaveChanges();

            return user;
        }

        public User UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();

            return user;
        }

        public User Find(int id)
        {
            return DbContext.Users.Include(u => u.Posts).SingleOrDefault(u => u.Id == id);
        }

        public bool DeleteUser(int id)
        {
            var userToDelete = Find(id);

            DbContext.Remove(userToDelete);

            return DbContext.SaveChanges() == 1;
        }
    }
}
