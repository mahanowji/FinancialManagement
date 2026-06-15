using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistants.Repositories
{
    internal class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task AddAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            client.UpdatedAt = DateTime.UtcNow;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            // Soft Delete
            client.IsDeleted = true;
            client.UpdatedAt = DateTime.UtcNow;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task<Client?> GetByEmailAsync(string email)
        {
            return await _context.Clients
                .Include(c => c.Household)
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Clients.CountAsync();
        }

        public async Task<int> GetActiveCountAsync()
        {
            return await _context.Clients.CountAsync(c => c.Status == ClientStatus.Active);
        }


        public async Task<List<Client>> GetAllClientUserAsync(Guid id)
        {
            return await _context.Clients.Where(x=>x.AdvisorId == id)
                .ToListAsync();
        }
    }
}