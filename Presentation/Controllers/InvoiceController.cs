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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public InvoiceController(
            IInvoiceService invoiceService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _invoiceService = invoiceService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Advisor")] 
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto)
        {
            var result = await _invoiceService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { invoiceId = result.Data, message = "Invoice created successfully" });
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {

            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to these invoices");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own invoices");
            }

            var result = await _invoiceService.GetByClientIdAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpPut("{invoiceId}/mark-paid")]
        [Authorize(Roles = "Admin,Advisor")]  
        public async Task<IActionResult> MarkAsPaid(Guid invoiceId)
        {
            var result = await _invoiceService.MarkAsPaidAsync(invoiceId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Invoice marked as paid successfully" });
        }
    }
}