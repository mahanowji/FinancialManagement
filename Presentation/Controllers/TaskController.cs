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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public TaskController(
            ITaskService taskService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _taskService = taskService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Advisor")]  // فقط Admin و Advisor
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var result = await _taskService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { taskId = result.Data, message = "Task created successfully" });
        }

        [HttpPut("{taskId}/complete")]
        [Authorize(Roles = "Admin,Advisor,Staff")]  // Admin, Advisor, Staff
        public async Task<IActionResult> Complete(Guid taskId)
        {
            var result = await _taskService.CompleteAsync(taskId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Task completed successfully" });
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
                    return Forbid("You don't have access to these tasks");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own tasks");
            }

            var result = await _taskService.GetByClientIdAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }
    }
}