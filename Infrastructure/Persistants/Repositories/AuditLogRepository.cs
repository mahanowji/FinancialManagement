using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistants.Repositories
{
    internal class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}