﻿using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public interface IUserService
    {
        User AddUser(User user);
        User UpdateUser(User user);
        void RemoveUser(User user);
        List<User> FetchAll();
    }
}
