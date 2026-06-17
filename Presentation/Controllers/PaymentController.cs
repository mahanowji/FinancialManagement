using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceService _invoiceService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public PaymentController(
            IPaymentService paymentService,
            IInvoiceService invoiceService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _paymentService = paymentService;
            _invoiceService = invoiceService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var result = await _paymentService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { paymentId = result.Data, message = "Payment recorded successfully" });
        }

        [HttpGet("invoice/{invoiceId}")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetByInvoiceId(Guid invoiceId)
        {

            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to these payments");


                var invoiceResult = await _invoiceService.GetByClientIdAsync(clientResult.Data.Id);

                if (!invoiceResult.IsSuccess || invoiceResult.Data == null)
                    return Forbid("You don't have access to these payments");

     
                var invoiceExists = invoiceResult.Data.Any(i => i.Id == invoiceId);

                if (!invoiceExists)
                    return Forbid("You can only view payments for your own invoices");
            }

            var result = await _paymentService.GetByInvoiceIdAsync(invoiceId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }
    }
}