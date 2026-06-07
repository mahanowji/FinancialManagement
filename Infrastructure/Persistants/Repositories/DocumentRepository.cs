using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistants.Repositories
{
    internal class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            // Soft delete
            document.IsDeleted = true;
            document.UpdatedAt = DateTime.UtcNow;

            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Document>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Documents
                .Where(x => x.ClientId == clientId && !x.IsDeleted)
                .Include(x => x.Category)
                .Include(x => x.Client)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }
}