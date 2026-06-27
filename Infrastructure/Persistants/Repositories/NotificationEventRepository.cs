// Infrastructure/Persistants/Repositories/NotificationEventRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Persistants.Repositories
{
    internal class NotificationEventRepository : INotificationEventRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationEvent?> GetByIdAsync(Guid id)
        {
            return await _context.NotificationEvents
                .Include(n => n.Client)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<NotificationEvent>> GetByClientIdAsync(Guid clientId)
        {
            return await _context.NotificationEvents
                .Where(n => n.ClientId == clientId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<NotificationEvent>> GetUnreadByClientIdAsync(Guid clientId)
        {
            return await _context.NotificationEvents
                .Where(n => n.ClientId == clientId && !n.IsRead)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task<List<NotificationEvent>> GetAllAsync()
        {
            return await _context.NotificationEvents
                .Include(n => n.Client)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

        public async Task AddAsync(NotificationEvent notificationEvent)
        {
            await _context.NotificationEvents.AddAsync(notificationEvent);
        }

        public Task UpdateAsync(NotificationEvent notificationEvent)
        {
            _context.NotificationEvents.Update(notificationEvent);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(NotificationEvent notificationEvent)
        {
            _context.NotificationEvents.Remove(notificationEvent);
            return Task.CompletedTask;
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await UpdateAsync(notification);
            }
        }
    }
}