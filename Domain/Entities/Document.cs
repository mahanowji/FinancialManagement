using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class Document : BaseEntity
{
    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public DocumentCategory Category { get; set; } = null!;
}