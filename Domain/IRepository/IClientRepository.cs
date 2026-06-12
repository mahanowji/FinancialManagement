using Domain.Entities;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);

    Task<List<Client>> GetAllAsync();

    Task AddAsync(Client client);

    Task UpdateAsync(Client client);

    Task DeleteAsync(Client client);

    Task<Client?> GetByEmailAsync(string email);
}