using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IGiftService
    {
        Task<Gift> AddGiftToUser(int userId, Gift gift);
        Task<List<Gift>> GetGiftsForUser(int userId);
        Task<Gift> UpdateGiftForUser(int userId, Gift gift);
        Task<bool> RemoveGift(Gift gift);
    }
}
