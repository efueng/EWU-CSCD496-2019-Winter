using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> ToReturn { get; set; }
        public int GetGiftsForUser_UserId { get; set; }

        public Task<Gift> AddGiftToUser(int userId, Gift gift)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public Task<List<Gift>> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return Task.FromResult(ToReturn);
        }

        public Task<bool> RemoveGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public Task<Gift> UpdateGiftForUser(int userId, Gift gift)
        {
            throw new NotImplementedException();
        }
    }
}
