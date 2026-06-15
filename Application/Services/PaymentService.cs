using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditLogService _auditLogService;
    private readonly IInvoiceService _invoiceService;

    public PaymentService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService,
        IInvoiceService invoiceService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _auditLogService = auditLogService;
        _invoiceService = invoiceService;
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreatePaymentDto dto)
    {
        try
        {

            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(dto.InvoiceId);
            if (invoice == null)
                return new ServiceResult<Guid>($"Invoice not found");


            if (invoice.Status == InvoiceStatus.Paid)
                return new ServiceResult<Guid>($"Invoice already paid");

     
            if (dto.Amount <= 0)
                return new ServiceResult<Guid>($"Payment amount must be greater than zero");

            if (dto.Amount > invoice.Amount)
                return new ServiceResult<Guid>($"Payment amount ({dto.Amount:C}) exceeds invoice amount ({invoice.Amount:C})");

      
            var payment = new PaymentRecord
            {
                Id = Guid.NewGuid(),
                InvoiceId = dto.InvoiceId,
                Amount = dto.Amount,
                Status = PaymentStatus.Paid,
                PaidAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,

            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.CommitChangesAsync();

 
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Payment of {dto.Amount:C} created for invoice '{invoice.InvoiceNumber}'",
                UserId = _currentUserService.GetCurrentUserId()
            });


            var markAsPaidResult = await _invoiceService.MarkAsPaidAsync(dto.InvoiceId);

            if (!markAsPaidResult.IsSuccess)
                return new ServiceResult<Guid>($"Payment recorded but invoice update failed: {markAsPaidResult.Message}");

            return new ServiceResult<Guid>(payment.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating payment: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<PaymentDto>>> GetByInvoiceIdAsync(Guid invoiceId)
    {
        try
        {
 
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                return new ServiceResult<List<PaymentDto>>($"Invoice not found");

            
            var payments = await _unitOfWork.PaymentRepository.GetByInvoiceIdAsync(invoiceId);


            var dtos = payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Status = p.Status,
                PaidAt = p.PaidAt
            }).ToList();

            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Payments viewed for invoice '{invoice.InvoiceNumber}' ({dtos.Count} payments)",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<List<PaymentDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<PaymentDto>>($"Error getting payments: {ex.Message}");
        }
    }
}