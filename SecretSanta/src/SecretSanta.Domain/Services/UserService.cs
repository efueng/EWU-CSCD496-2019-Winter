using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SecretSanta.Domain.Services
{
    public class UserService
    {
        private ApplicationDbContext DbContext { get; }
        public UserService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public User AddUser(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            return user;
        }

        public void UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();
        }

        public User Find(int id)
        {
            return DbContext.Users.Include(u => u.Gifts).SingleOrDefault(u => u.Id == id);
        }

        public List<User> FetchAll()
        {
            var userTask = DbContext.Users.ToListAsync();
            userTask.Wait();

            return userTask.Result;
        }
    }
}
