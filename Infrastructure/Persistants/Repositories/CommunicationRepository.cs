using CmsKit.Infrastructure;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistants.Repositories
{
    internal class CommunicationRepository : ICommunicationRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Communication communication)
        {
            if (communication == null)
                throw new ArgumentNullException(nameof(communication));

            await _context.Communications.AddAsync(communication);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Communication>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Communications
                .Where(x => x.ClientId == clientId && !x.IsDeleted)
                .ToListAsync();
        }
    }
}