using BlogEngine.Domain.Models;
using BlogEngine.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Domain.Services
{
    public class TagService : ITagService
    {
        private ApplicationDbContext DbContext { get; }
        public TagService(ApplicationDbContext context)
        {
            DbContext = context;
        }
    }
}
