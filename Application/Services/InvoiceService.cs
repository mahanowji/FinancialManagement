using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Enums;
using Domain.Interfaces;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditLogService _auditLogService;

    public InvoiceService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateInvoiceDto dto)
    {
        try
        {
      
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult<Guid>($"Client not found");


            var existingInvoice = await _unitOfWork.InvoiceRepository
                .GetByNumberAsync(dto.InvoiceNumber);

            if (existingInvoice != null)
                return new ServiceResult<Guid>($"Invoice number '{dto.InvoiceNumber}' already exists");

      
            var invoice = new Domain.Entities.Invoice
            {
                Id = Guid.NewGuid(),
                ClientId = dto.ClientId,
                InvoiceNumber = dto.InvoiceNumber,
                Amount = dto.Amount,
                Status = InvoiceStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InvoiceRepository.AddAsync(invoice);
            await _unitOfWork.CommitChangesAsync();

            
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Invoice '{invoice.InvoiceNumber}' created for client {client.FirstName} {client.LastName} with amount {invoice.Amount:C}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(invoice.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating invoice: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<InvoiceDto>>> GetByClientIdAsync(Guid clientId)
    {
        try
        {
           
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
            if (client == null)
                return new ServiceResult<List<InvoiceDto>>($"Client not found");

    
            var invoices = await _unitOfWork.InvoiceRepository.GetByClientIdAsync(clientId);

      
            var dtos = invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                Amount = i.Amount,
                Status = i.Status
            }).ToList();

      
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Invoices viewed for client {client.FirstName} {client.LastName} ({dtos.Count} invoices)",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<List<InvoiceDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<InvoiceDto>>($"Error getting invoices: {ex.Message}");
        }
    }

    public async Task<ServiceResult> MarkAsPaidAsync(Guid invoiceId)
    {
        try
        {
      
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                return new ServiceResult($"Invoice not found");

     
            if (invoice.Status == InvoiceStatus.Paid)
                return new ServiceResult($"Invoice already paid");

        
            invoice.Status = InvoiceStatus.Paid;


            await _unitOfWork.InvoiceRepository.UpdateAsync(invoice);
            await _unitOfWork.CommitChangesAsync();

    
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Invoice '{invoice.InvoiceNumber}' marked as paid. Amount: {invoice.Amount:C}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }
        catch (Exception ex)
        {
            return new ServiceResult($"Error marking invoice as paid: {ex.Message}");
        }
    }
}