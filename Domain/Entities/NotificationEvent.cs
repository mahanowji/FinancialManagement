using Domain.Abstractions;

namespace Domain.Entities;

public class NotificationEvent : BaseEntity
{
    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
}