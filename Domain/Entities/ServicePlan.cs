using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class ServicePlan : BaseEntity
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();
}