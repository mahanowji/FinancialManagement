// Application/Common/Interfaces/INotificationService.cs
using Application.Common.Dto;
using Domain.Abstractions;

namespace Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task<ServiceResult<Guid>> SendToClientAsync(SendNotificationDto dto);
        Task<ServiceResult<List<Guid>>> SendToMultipleClientsAsync(SendMultipleNotificationDto dto);
        Task<ServiceResult<NotificationDto>> GetByIdAsync(Guid notificationId);
        Task<ServiceResult<List<NotificationDto>>> GetClientNotificationsAsync(Guid clientId);
        Task<ServiceResult<List<NotificationDto>>> GetUnreadNotificationsAsync(Guid clientId);
        Task<ServiceResult<int>> GetUnreadCountAsync(Guid clientId);
        Task<ServiceResult> MarkAsReadAsync(Guid notificationId);
        Task<ServiceResult> MarkAllAsReadAsync(Guid clientId);
        Task<ServiceResult> DeleteAsync(Guid notificationId);
        Task<ServiceResult> DeleteAllForClientAsync(Guid clientId);
    }
}