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
        private IPairingService Service { get; set; }
        public PairingService(ApplicationDbContext dbContext, IPairingService service)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Service = service;
        }
        public async Task<bool> GeneratePairings(int groupId)
        {
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if (userIds == null || userIds.Count < 2)
            {
                return false;
            }

            // Invoke GetPairings on separate thread
            Task<List<Pairing>> task = Task.Run(() => GetPairings(userIds));
            List<Pairing> pairings = await task;

            await DbContext.AddRangeAsync();
            await DbContext.SaveChangesAsync();

            return true;
        }

        private List<Pairing> GetPairings(List<int> userIds)
        {
            var pairings = new List<Pairing>();

            for (int idx = 0; idx < userIds.Count - 1; idx++)
            {
                var pairing = new Pairing
                {
                    SantaId = userIds[idx],
                    RecipientId = userIds[idx + 1]
                };
            }

            var lastPairing = new Pairing
            {
                SantaId = userIds.Last(),
                RecipientId = userIds.First()
            };

            pairings.Add(lastPairing);

            return pairings;
        }
    }
}
