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
        public async Task GeneratePairings(int groupId)
        {
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            List<int> userIds = group?.GroupUsers?.Select(x => x.UserId).ToList();

            // Invoke GetPairings on separate thread
            await GetPairings(userIds);
        }

        private async Task<List<Pairing>> GetPairings(List<int> userIds)
        {
            return null;
        }
    }
}
