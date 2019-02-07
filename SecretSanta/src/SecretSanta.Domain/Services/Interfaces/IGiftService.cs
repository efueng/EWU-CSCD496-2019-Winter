using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IGiftService
    {
        List<Gift> GetGiftsForUser(int userId);
    }
}
