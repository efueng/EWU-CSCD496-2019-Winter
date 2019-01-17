using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class MessageService
    {
        private ApplicationDbContext DbContext { get; set; }
        public MessageService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        
        public void AddMessage(Message message)
        {
            DbContext.Messages.Add(message);
        }

        public Message Find(int id)
        {
            return DbContext.Messages.Include(m => m.Recipient)
                .Include(m => m.Santa)
                .SingleOrDefault(m => m.Id == id);
        }
    }
}
