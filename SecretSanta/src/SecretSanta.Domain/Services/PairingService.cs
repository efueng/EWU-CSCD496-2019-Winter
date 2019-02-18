using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{
    public class PairingService : IPairingService
    {
        private ApplicationDbContext DbContext { get; set; }
        private ThreadSafeRandom Random { get; set; }
        public PairingService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Random = new ThreadSafeRandom();
        }

        public async Task<List<Pairing>> GeneratePairings(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentOutOfRangeException("groupId must be at least 1.", nameof(groupId));
            }

            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if (userIds == null || userIds.Count < 2)
            {
                return null;
            }

            // Invoke GetPairings on separate thread
            Task<List<Pairing>> task = Task.Run(() => GetPairings(userIds, groupId));
            List<Pairing> pairings = await task;

            await DbContext.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();

            return pairings;
        }

        private List<Pairing> GetPairings(List<int> userIds, int groupId)
        {
            // this was stolen from Kenny and Casey White
            var randomizedIds = userIds.OrderBy(id => Random.Next()).ToList();
            var pairings = new List<Pairing>();

            for (int idx = 0; idx < userIds.Count - 1; idx++)
            {
                var pairing = new Pairing
                {
                    SantaId = randomizedIds[idx],
                    RecipientId = randomizedIds[idx + 1],
                    OriginGroupId = groupId
                };

                pairings.Add(pairing);
            }

            var lastPairing = new Pairing
            {
                SantaId = randomizedIds.Last(),
                RecipientId = randomizedIds.First(),
                OriginGroupId = groupId
            };

            pairings.Add(lastPairing);

            return pairings;
        }
    }
}
