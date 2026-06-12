using System.Threading.Tasks;
using Domain.Abstractions;

public interface IPaymentService
{
    Task<ServiceResult<Guid>> CreateAsync(CreatePaymentDto dto);

    Task<ServiceResult<List<PaymentDto>>> GetByInvoiceIdAsync(Guid invoiceId);
}