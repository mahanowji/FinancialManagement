// Domain/Interfaces/INotificationEventRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface INotificationEventRepository
    {
        Task<NotificationEvent?> GetByIdAsync(Guid id);
        Task<List<NotificationEvent>> GetByClientIdAsync(Guid clientId);
        Task<List<NotificationEvent>> GetUnreadByClientIdAsync(Guid clientId);
        Task<List<NotificationEvent>> GetAllAsync();
        Task AddAsync(NotificationEvent notificationEvent);
        Task UpdateAsync(NotificationEvent notificationEvent);
        Task DeleteAsync(NotificationEvent notificationEvent);
        Task MarkAsReadAsync(Guid id);
    }
}