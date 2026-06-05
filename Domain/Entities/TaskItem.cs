using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
}