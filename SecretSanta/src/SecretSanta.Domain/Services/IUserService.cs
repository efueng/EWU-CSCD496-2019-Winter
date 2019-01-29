using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    interface IUserService
    {
        User AddUser(User user);
        User UpdateUser(User user);
        List<User> FetchAll();
    }
}
