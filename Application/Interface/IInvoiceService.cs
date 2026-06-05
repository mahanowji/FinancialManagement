using Domain.Abstractions;

public interface IInvoiceService
{
    Task<ServiceResult<Guid>>
        CreateAsync(CreateInvoiceDto dto);

    Task<ServiceResult<List<InvoiceDto>>>
        GetByClientIdAsync(Guid clientId);

    Task<ServiceResult>
        MarkAsPaidAsync(Guid invoiceId);
}