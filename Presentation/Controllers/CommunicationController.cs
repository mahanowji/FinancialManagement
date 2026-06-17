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
    public class CommunicationController : ControllerBase
    {
        private readonly ICommunicationService _communicationService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public CommunicationController(
            ICommunicationService communicationService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _communicationService = communicationService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<IActionResult> Create([FromBody] CreateCommunicationDto dto)
        {
            var result = await _communicationService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { communicationId = result.Data, message = "Communication recorded successfully" });
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetClientHistory(Guid clientId)
        {

            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to this client's communications");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own communication history");
            }

            var result = await _communicationService.GetClientHistoryAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }
    }
}