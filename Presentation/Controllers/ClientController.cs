using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // همه متدها نیاز به احراز هویت دارند
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public ClientController(
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        #region Client Management

        [HttpPost]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
        {
            var result = await _clientService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { clientId = result.Data, message = "Client created successfully" });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Advisor,Staff")]  // Admin, Advisor, Staff
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _clientService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Advisor,Staff")]  
        public async Task<IActionResult> GetAll()
        {
            var result = await _clientService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientDto dto)
        {
            var result = await _clientService.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Client updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _clientService.DeleteAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Client deleted successfully" });
        }

        #endregion

        #region Service Plan Management

        [HttpPost("service-plan")]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> CreateServicePlan([FromBody] CreateServicePlanDto dto)
        {
            var result = await _clientService.CreateServicePlanAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { servicePlanId = result.Data, message = "Service plan created successfully" });
        }

        [HttpGet("service-plans")]
        [Authorize(Roles = "Admin,Advisor,Staff")]  // Admin, Advisor, Staff
        public async Task<IActionResult> GetServicePlans()
        {
            var result = await _clientService.GetServicePlansAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        #endregion

        #region Consent Management

        [HttpPost("consent")]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> AddConsent([FromBody] CreateConsentDto dto)
        {
            var result = await _clientService.AddConsentAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Consent added successfully" });
        }

        #endregion

    }
}