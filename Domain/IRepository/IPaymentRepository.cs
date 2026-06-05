using Domain.Entities;

public interface IPaymentRepository
{
    Task AddAsync(PaymentRecord payment);

    Task<List<PaymentRecord>>
        GetByInvoiceIdAsync(Guid invoiceId);
}