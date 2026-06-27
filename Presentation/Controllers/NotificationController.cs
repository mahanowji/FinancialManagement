using Application.Common.Dto;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(
            INotificationService notificationService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _notificationService = notificationService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<IActionResult> SendToClient([FromBody] SendNotificationDto dto)
        {
            var result = await _notificationService.SendToClientAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { notificationId = result.Data, message = "Notification sent successfully" });
        }

        [HttpPost("send-bulk")]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<IActionResult> SendToMultipleClients([FromBody] SendMultipleNotificationDto dto)
        {
            var result = await _notificationService.SendToMultipleClientsAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { notificationIds = result.Data, message = "Notifications sent successfully" });
        }

        [HttpGet("{notificationId}")]
        [Authorize(Roles = "Admin,Advisor,Staff")]
        public async Task<IActionResult> GetById(Guid notificationId)
        {
            var result = await _notificationService.GetByIdAsync(notificationId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetClientNotifications(Guid clientId)
        {
            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to these notifications");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own notifications");
            }

            var result = await _notificationService.GetClientNotificationsAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("client/{clientId}/unread")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetUnreadNotifications(Guid clientId)
        {
            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to these notifications");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own notifications");
            }

            var result = await _notificationService.GetUnreadNotificationsAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("client/{clientId}/unread-count")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetUnreadCount(Guid clientId)
        {
            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to this information");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own notification count");
            }

            var result = await _notificationService.GetUnreadCountAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpPut("{notificationId}/read")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var result = await _notificationService.MarkAsReadAsync(notificationId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Notification marked as read successfully" });
        }

        [HttpPut("client/{clientId}/read-all")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> MarkAllAsRead(Guid clientId)
        {
            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to this action");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only mark your own notifications as read");
            }

            var result = await _notificationService.MarkAllAsReadAsync(clientId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "All notifications marked as read successfully" });
        }

        [HttpDelete("{notificationId}")]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<IActionResult> Delete(Guid notificationId)
        {
            var result = await _notificationService.DeleteAsync(notificationId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Notification deleted successfully" });
        }

        [HttpDelete("client/{clientId}")]
        [Authorize(Roles = "Admin,Advisor")]
        public async Task<IActionResult> DeleteAllForClient(Guid clientId)
        {
            var result = await _notificationService.DeleteAllForClientAsync(clientId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "All notifications deleted successfully" });
        }
    }
}