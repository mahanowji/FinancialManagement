using Domain.Entities;

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice);

    Task<Invoice?> GetByIdAsync(Guid id);

    Task<List<Invoice>>
        GetByClientIdAsync(Guid clientId);
}