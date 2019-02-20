using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Domain.Services
{
    public class GiftService : IGiftService
    {
        private ApplicationDbContext DbContext { get; }

        public GiftService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

<<<<<<< refs/remotes/intellitect/Assignment6
        public async Task<Gift> AddGiftToUser(int userId, Gift gift)
=======
        public async Task<Gift> AddGift(Gift gift)
>>>>>>> Initial start of code for assignment 7
        {
            if (gift == null) throw new ArgumentNullException(nameof(gift));

            DbContext.Gifts.Add(gift);
            await DbContext.SaveChangesAsync();

            return gift;
        }

<<<<<<< refs/remotes/intellitect/Assignment6
        public async Task<Gift> UpdateGiftForUser(int userId, Gift gift)
=======
        public async Task<Gift> GetGift(int giftId)
        {
            var retrievedGift = await DbContext.Gifts.SingleOrDefaultAsync(g => g.Id == giftId);

            return retrievedGift;
        }

        public async Task<Gift> UpdateGift(Gift gift)
>>>>>>> Initial start of code for assignment 7
        {
            if (gift == null) throw new ArgumentNullException(nameof(gift));

            DbContext.Gifts.Update(gift);
            await DbContext.SaveChangesAsync();

            return gift;
        }

        public async Task<List<Gift>> GetGiftsForUser(int userId)
        {
            return await DbContext.Gifts.Where(g => g.UserId == userId).ToListAsync();
        }

<<<<<<< refs/remotes/intellitect/Assignment6
        public async Task RemoveGift(Gift gift)
=======
        public async Task RemoveGift(int giftId)
>>>>>>> Initial start of code for assignment 7
        {
            var giftToDelete = await DbContext.Gifts.FindAsync(giftId);

<<<<<<< refs/remotes/intellitect/Assignment6
            DbContext.Gifts.Remove(gift);
            await DbContext.SaveChangesAsync();
=======
            if (giftToDelete != null)
            {
                DbContext.Gifts.Remove(giftToDelete);
                DbContext.SaveChanges();
            }
>>>>>>> Initial start of code for assignment 7
        }
    }
}