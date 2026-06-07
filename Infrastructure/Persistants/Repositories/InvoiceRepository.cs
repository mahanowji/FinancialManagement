using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<Invoice>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Invoices
            .Where(x => x.ClientId == clientId && !x.IsDeleted)
            .ToListAsync();
    }
}