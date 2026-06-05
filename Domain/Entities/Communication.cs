using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Communication : BaseEntity
{
    public CommunicationType Type { get; set; }

    public string Description { get; set; } = null!;

    public DateTime OccurredAt { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
}