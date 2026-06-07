using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentRecord payment)
    {
        await _context.PaymentRecords.AddAsync(payment);
    }

    public async Task<List<PaymentRecord>> GetByInvoiceIdAsync(Guid invoiceId)
    {
        return await _context.PaymentRecords
            .Where(x => x.InvoiceId == invoiceId)
            .ToListAsync();
    }
}