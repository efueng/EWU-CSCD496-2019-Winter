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

<<<<<<< refs/remotes/intellitect/Assignment6
=======
        public Task<Gift> AddGift(Gift gift)
        {
            throw new NotImplementedException();
        }

        public Task<Gift> GetGift(int giftId)
        {
            throw new NotImplementedException();
        }

>>>>>>> Initial start of code for assignment 7
        public Task<List<Gift>> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return Task.FromResult(ToReturn);
<<<<<<< refs/remotes/intellitect/Assignment6
=======
        }

        public Task RemoveGift(int giftId)
        {
            throw new NotImplementedException();
        }

        public Task<Gift> UpdateGift(Gift gift)
        {
            throw new NotImplementedException();
>>>>>>> Initial start of code for assignment 7
        }
    }
}
