// Infrastructure/Repositories/ConsentRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistants.Repositories
{
    public class ConsentRepository : IConsentRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Consent?> GetByIdAsync(Guid id)
        {
            return await _context.Consents
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Consent>> GetAllAsync()
        {
            return await _context.Consents
                .Include(c => c.Client)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Consent>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Consents
                .Where(c => c.ClientId == clientId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Consent>> GetValidConsentsAsync(Guid clientId)
        {
            return await _context.Consents
                .Where(c => c.ClientId == clientId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Consent>> GetByTypeAsync(string consentType)
        {
            return await _context.Consents
                .Include(c => c.Client)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Consent consent)
        {
            await _context.Consents.AddAsync(consent);
        }

        public Task UpdateAsync(Consent consent)
        {
            _context.Consents.Update(consent);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Consent consent)
        {
            _context.Consents.Remove(consent);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Consents.AnyAsync(c => c.Id == id);
        }
    }
}