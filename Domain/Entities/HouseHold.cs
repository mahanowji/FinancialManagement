using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class Household : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Client> Clients { get; set; } = new List<Client>();
}