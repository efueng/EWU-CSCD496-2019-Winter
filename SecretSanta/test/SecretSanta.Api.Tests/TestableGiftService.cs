﻿using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;
using System.Linq;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> GetGiftsForUser_Return { get; set; }
        public int GetGiftsForUser_UserId { get; set; }
        public Gift AddGiftToUser_Gift { get; set; }
        public int AddGiftToUser_UserId { get; set; }

        public Gift AddGiftToUser(int userId, Gift gift)
        {
            throw new System.NotImplementedException();
        }

        public List<Gift> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            GetGiftsForUser_Return = GetGiftsForUser(userId);
            return GetGiftsForUser_Return;
        }

        public void RemoveGift(Gift gift)
        {
            throw new System.NotImplementedException();
        }

        public Gift UpdateGiftForUser(int userId, Gift gift)
        {
            throw new System.NotImplementedException();
        }
    }
}