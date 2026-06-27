// Application/Services/NotificationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditLogService _auditLogService;

        public NotificationService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _auditLogService = auditLogService;
        }

        public async Task<ServiceResult<Guid>> SendToClientAsync(SendNotificationDto dto)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
                if (client == null)
                    return new ServiceResult<Guid>("Client not found");

                var notification = new NotificationEvent
                {
                    Id = Guid.NewGuid(),
                    ClientId = dto.ClientId,
                    Title = dto.Title,
                    Message = dto.Message,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                await _unitOfWork.NotificationEventRepository.AddAsync(notification);
                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Notification '{dto.Title}' sent to client {client.FirstName} {client.LastName}",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult<Guid>(notification.Id);
            }
            catch (Exception ex)
            {
                return new ServiceResult<Guid>($"Error sending notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Guid>>> SendToMultipleClientsAsync(SendMultipleNotificationDto dto)
        {
            try
            {
                var notificationIds = new List<Guid>();

                foreach (var clientId in dto.ClientIds)
                {
                    var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                    if (client == null)
                        continue;

                    var notification = new NotificationEvent
                    {
                        Id = Guid.NewGuid(),
                        ClientId = clientId,
                        Title = dto.Title,
                        Message = dto.Message,
                        SentAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    await _unitOfWork.NotificationEventRepository.AddAsync(notification);
                    notificationIds.Add(notification.Id);
                }

                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Bulk notification '{dto.Title}' sent to {notificationIds.Count} clients",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult<List<Guid>>(notificationIds);
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<Guid>>($"Error sending bulk notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<NotificationDto>> GetByIdAsync(Guid notificationId)
        {
            try
            {
                var notification = await _unitOfWork.NotificationEventRepository.GetByIdAsync(notificationId);
                if (notification == null)
                    return new ServiceResult<NotificationDto>("Notification not found");

                var dto = new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    SentAt = notification.SentAt,
                    ClientName = notification.Client != null
                        ? $"{notification.Client.FirstName} {notification.Client.LastName}"
                        : "Unknown"
                };

                return new ServiceResult<NotificationDto>(dto);
            }
            catch (Exception ex)
            {
                return new ServiceResult<NotificationDto>($"Error getting notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<NotificationDto>>> GetClientNotificationsAsync(Guid clientId)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return new ServiceResult<List<NotificationDto>>("Client not found");

                var notifications = await _unitOfWork.NotificationEventRepository
                    .GetByClientIdAsync(clientId);

                var dtos = notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    SentAt = n.SentAt,
                    ClientName = $"{client.FirstName} {client.LastName}"
                }).ToList();

                return new ServiceResult<List<NotificationDto>>(dtos);
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<NotificationDto>>($"Error getting notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<NotificationDto>>> GetUnreadNotificationsAsync(Guid clientId)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return new ServiceResult<List<NotificationDto>>("Client not found");

                var notifications = await _unitOfWork.NotificationEventRepository
                    .GetUnreadByClientIdAsync(clientId);

                var dtos = notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    SentAt = n.SentAt,
                    ClientName = $"{client.FirstName} {client.LastName}"
                }).ToList();

                return new ServiceResult<List<NotificationDto>>(dtos);
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<NotificationDto>>($"Error getting unread notifications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetUnreadCountAsync(Guid clientId)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return new ServiceResult<int>("Client not found");

                var unread = await _unitOfWork.NotificationEventRepository
                    .GetUnreadByClientIdAsync(clientId);

                return new ServiceResult<int>(unread.Count);
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>($"Error getting unread count: {ex.Message}");
            }
        }

        public async Task<ServiceResult> MarkAsReadAsync(Guid notificationId)
        {
            try
            {
                var notification = await _unitOfWork.NotificationEventRepository.GetByIdAsync(notificationId);
                if (notification == null)
                    return new ServiceResult("Notification not found");

                if (notification.IsRead)
                    return new ServiceResult("Notification already read");

                notification.IsRead = true;

                await _unitOfWork.NotificationEventRepository.UpdateAsync(notification);
                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Notification '{notification.Title}' marked as read",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                return new ServiceResult($"Error marking notification as read: {ex.Message}");
            }
        }

        public async Task<ServiceResult> MarkAllAsReadAsync(Guid clientId)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return new ServiceResult("Client not found");

                var unreadNotifications = await _unitOfWork.NotificationEventRepository
                    .GetUnreadByClientIdAsync(clientId);

                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                    await _unitOfWork.NotificationEventRepository.UpdateAsync(notification);
                }

                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"All notifications marked as read for client {client.FirstName} {client.LastName}",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                return new ServiceResult($"Error marking all as read: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteAsync(Guid notificationId)
        {
            try
            {
                var notification = await _unitOfWork.NotificationEventRepository.GetByIdAsync(notificationId);
                if (notification == null)
                    return new ServiceResult("Notification not found");

                await _unitOfWork.NotificationEventRepository.DeleteAsync(notification);
                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Notification '{notification.Title}' deleted",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                return new ServiceResult($"Error deleting notification: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteAllForClientAsync(Guid clientId)
        {
            try
            {
                var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
                if (client == null)
                    return new ServiceResult("Client not found");

                var notifications = await _unitOfWork.NotificationEventRepository
                    .GetByClientIdAsync(clientId);

                foreach (var notification in notifications)
                {
                    await _unitOfWork.NotificationEventRepository.DeleteAsync(notification);
                }

                await _unitOfWork.CommitChangesAsync();

                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"All notifications deleted for client {client.FirstName} {client.LastName}",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                return new ServiceResult($"Error deleting notifications: {ex.Message}");
            }
        }
    }
}