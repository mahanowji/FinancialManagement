using Domain.Entities;
using Domain.Enums;
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


    public Task UpdateAsync(Invoice invoice)
    {
        _context.Invoices.Update(invoice);

        return Task.CompletedTask;
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

    public async Task<int> GetUnpaidCountAsync()
    {
        return await _context.Invoices.CountAsync(i => i.Status == InvoiceStatus.Pending);
    }

    public async Task<decimal> GetUnpaidTotalAmountAsync()
    {
        return await _context.Invoices
            .Where(i => i.Status == InvoiceStatus.Pending)
            .SumAsync(i => i.Amount);
    }


    public async Task<Invoice?> GetByNumberAsync(string invoiceNumber)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
    }
}