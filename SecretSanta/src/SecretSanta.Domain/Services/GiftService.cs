using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GiftService
    {
        private ApplicationDbContext DbContext { get; set; }
        public GiftService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Gift AddGift(Gift gift)
        {
            DbContext.Gifts.Add(gift);
            DbContext.SaveChanges();

            return gift;
        }

        public void AddGiftForUser(int userId, Gift gift)
        {
            User user = DbContext.Users.Find(userId);
            user.Gifts.Add(gift);

            DbContext.SaveChanges();
        }

        public void RemoveGiftForUser(int userId, Gift gift)
        {
            User user = DbContext.Users.Find(userId);
            user.Gifts.Remove(gift);

            DbContext.SaveChanges();
        }

        public void UpdateGift(Gift gift)
        {
            DbContext.Gifts.Update(gift);
            DbContext.SaveChanges();
        }

        public Gift Find(int id)
        {
            return DbContext.Gifts.Include(g => g.User).SingleOrDefault(g => g.Id == id);
        }
    }
}
