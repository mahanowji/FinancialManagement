using System;
using Application.Common.Dto;
using Domain.Abstractions;
using Domain.Entities;

public interface IClientService
{
    Task<ServiceResult<Guid>> CreateAsync(CreateClientDto dto);

    Task<ServiceResult<ClientDto>> GetByIdAsync(Guid id);

    Task<ServiceResult<List<ClientDto>>> GetAllAsync();

    Task<ServiceResult> UpdateAsync(
        Guid id,
        UpdateClientDto dto);


    Task<ServiceResult<Guid>>
    CreateServicePlanAsync(CreateServicePlanDto dto);

    Task<ServiceResult<List<ServicePlanDto>>>
        GetServicePlansAsync();
    Task<ServiceResult>
    AddConsentAsync(CreateConsentDto dto);
    Task<ServiceResult> DeleteAsync(Guid id);
}