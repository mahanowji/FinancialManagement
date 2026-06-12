using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Client : BaseEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public ClientStatus Status { get; set; }

    public Guid AdvisorId { get; set; }

    public User Advisor { get; set; } = null!;

    public Guid? HouseholdId { get; set; }

    public Household? Household { get; set; }

    public Guid? ServicePlanId { get; set; }

    public ServicePlan? ServicePlan { get; set; }

    public ICollection<Note> Notes { get; set; } = new List<Note>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Communication> Communications { get; set; } = new List<Communication>();

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public ICollection<Consent> Consents { get; set; } = new List<Consent>();

    public ICollection<NotificationEvent> NotificationEvents { get; set; } = new List<NotificationEvent>();
}