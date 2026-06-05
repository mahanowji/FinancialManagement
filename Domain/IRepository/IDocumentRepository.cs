

using Domain.Entities;

public interface IDocumentRepository
{
    Task AddAsync(Document document);

    Task<Document?> GetByIdAsync(Guid id);

    Task<List<Document>>
        GetByClientIdAsync(Guid clientId);

    Task DeleteAsync(Document document);
}