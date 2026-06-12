
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IConsentRepository
    {
        Task<Consent?> GetByIdAsync(Guid id);
        Task<List<Consent>> GetAllAsync();
        Task<List<Consent>> GetByClientIdAsync(Guid clientId);
        Task<List<Consent>> GetValidConsentsAsync(Guid clientId);
        Task<List<Consent>> GetByTypeAsync(string consentType);
        Task AddAsync(Consent consent);
        Task UpdateAsync(Consent consent);
        Task DeleteAsync(Consent consent);
        Task<bool> ExistsAsync(Guid id);
    }
}