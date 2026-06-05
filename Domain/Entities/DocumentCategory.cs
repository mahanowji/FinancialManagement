using CmsKit.Domain.Abstractions;
using System.Reflection.Metadata;

namespace FinancialAdvisor.Domain.Entities;

public class DocumentCategory : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}