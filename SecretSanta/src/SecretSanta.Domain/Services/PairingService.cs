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
        //private IPairingService Service { get; set; }
        private RandomService Random { get; set; }
        public PairingService(ApplicationDbContext dbContext)//, IPairingService service, IRandomService random)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            //Service = service ?? throw new ArgumentNullException(nameof(service));
            Random = new RandomService();// random ?? throw new ArgumentNullException(nameof(random));
        }
        public async Task<List<Pairing>> GeneratePairings(int groupId)
        {
            //id is primary key
            //singeordefault will throw if 2 or more
            //firstordefault could grab 0 or 1 rows, but don't throw
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if (userIds == null || userIds.Count < 2)
            {
                return null;
            }

            // Invoke GetPairings on separate thread
            /* equivalent
	        foreach (Pairing pair in myPairings)
	        {
	            await DbContext.Pairings.AddAsync(pair);
	        }
	        */
            Task<List<Pairing>> task = Task.Run(() => GetPairings(userIds, groupId));
            List<Pairing> pairings = await task;

            await DbContext.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();

            return pairings;
        }

        private List<Pairing> GetPairings(List<int> userIds, int groupId)
        {
            int index = 0;
            int maxValue = userIds.Count - 1;
            var indices = Enumerable.Range(0, userIds.Count).ToList();
            var randomIndices = new List<int>();

            var pairings = new List<Pairing>();

            for (int idx = 0; idx < userIds.Count; idx++)
            {
                index = Random.Next(maxValue--);

                if (randomIndices.Contains(index))
                {
                    idx--;
                    maxValue++;
                }
                else
                {
                    randomIndices.Add(index);
                }
            }

            for (int idx = 0; idx < userIds.Count - 1; idx++)
            {
                var pairing = new Pairing
                {
                    SantaId = randomIndices[idx],
                    RecipientId = randomIndices[idx + 1],
                    OriginGroupId = groupId
                };

                pairings.Add(pairing);
            }

            var lastPairing = new Pairing
            {
                SantaId = randomIndices.Last(),
                RecipientId = randomIndices.First(),
                OriginGroupId = groupId
            };

            pairings.Add(lastPairing);

            return pairings;
        }
    }
}
