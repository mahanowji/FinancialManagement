using Domain.Entities;

public interface ICommunicationRepository
{
    Task AddAsync(Communication communication);

    Task<List<Communication>>
        GetByClientIdAsync(Guid clientId);
}