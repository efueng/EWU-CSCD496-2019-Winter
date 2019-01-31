using BlogEngine.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Domain.Services.Interfaces
{
    public interface IUserService
    {
        User AddUser(User user);

        User Find(int id);

        User UpdateUser(User user);

        bool DeleteUser(int id);
    }
}
