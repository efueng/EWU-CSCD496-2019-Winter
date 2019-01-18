using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class PairingService
    {
        private ApplicationDbContext DbContext { get; set; }

        public PairingService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Pairing AddPairing(Pairing pairing)
        {
            DbContext.Pairings.Add(pairing);
            DbContext.SaveChanges();

            return pairing;
        }

        public Pairing Find(int id)
        {
            return DbContext.Pairings.Include(p => p.Recipient)
                .Include(p => p.Santa)
                .SingleOrDefault(p => p.Id == id);
        }
    }
}
