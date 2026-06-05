using Application.Common.Dto;
using Domain.Abstractions;

public interface ICommunicationService
{
    Task<ServiceResult<Guid>>
        CreateAsync(CreateCommunicationDto dto);

    Task<ServiceResult<List<CommunicationDto>>>
        GetClientHistoryAsync(Guid clientId);
}