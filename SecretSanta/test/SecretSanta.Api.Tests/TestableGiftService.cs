using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;
using System.Linq;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public Gift AddGiftToUser_Gift { get; set; }
        public int AddGiftToUser_UserId { get; set; }
        public Gift AddGiftToUser(int userId, Gift gift)
        {
            throw new System.NotImplementedException();
        }


        public List<Gift> GetGiftsForUser_Return { get; set; }
        public int GetGiftsForUser_UserId { get; set; }
        public List<Gift> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return GetGiftsForUser_Return;
        }


        public Gift RemoveGift_Gift { get; set; }
        public void RemoveGift(Gift gift)
        {
            RemoveGift_Gift = gift;
        }


        public int UpdateGiftForUser_userId { get; set; }
        public Gift UpdateGiftForUser_Gift { get; set; }
        public Gift UpdateGiftForUser(int userId, Gift gift)
        {
            UpdateGiftForUser_userId = userId;
            UpdateGiftForUser_Gift = gift;

            return UpdateGiftForUser_Gift;
        }
    }
}