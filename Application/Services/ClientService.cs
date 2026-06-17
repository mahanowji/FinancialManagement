using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using System.Reflection;

namespace Application.Services
{
    internal class ClientService : IClientService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly ICurrentUserService _currentUserService;

        public ClientService(
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _currentUserService = currentUserService;
        }


        public async Task<ServiceResult> AddConsentAsync(CreateConsentDto dto)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult("Client not found");

            var consent = new Consent
            {
                Id = Guid.NewGuid(),
                ClientId = dto.ClientId,
                Description = dto.Description,
                Accepted = dto.Accepted,    
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ConsentRepository.AddAsync(consent);
            await _unitOfWork.CommitChangesAsync();

            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Consent added for client {client.FirstName} {client.LastName}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateClientDto dto)
        {

            var existingClient = await _unitOfWork.ClientRepository.GetByEmailAsync(dto.Email);

            if (existingClient != null)
                return new ServiceResult<Guid>("Client with this email already exists");

   
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Status = Domain.Enums.ClientStatus.Active,
                HouseholdId = dto.HouseholdId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ClientRepository.AddAsync(client);
            await _unitOfWork.CommitChangesAsync();

     
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client '{client.FirstName} {client.LastName}' created with email {client.Email}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(client.Id);
        }

        public async Task<ServiceResult<Guid>> CreateServicePlanAsync(CreateServicePlanDto dto)
        {
            var servicePlan = new ServicePlan
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ServicePlanRepository.AddAsync(servicePlan);
            await _unitOfWork.CommitChangesAsync();

            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Service plan '{servicePlan.Name}' created with monthly fee {servicePlan.Price}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(servicePlan.Id);
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            if (client == null)
                return new ServiceResult("Client not found");

            var clientName = $"{client.FirstName} {client.LastName}";

            await _unitOfWork.ClientRepository.DeleteAsync(client);
            await _unitOfWork.CommitChangesAsync();

        
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client '{clientName}' deleted",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }

        public async Task<ServiceResult<List<ClientDto>>> GetAllAsync()
        {
            var clients = await _unitOfWork.ClientRepository.GetAllAsync();

            var dtos = clients.Select(client => new ClientDto
            {
                Id = client.Id,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                FullName = $"{client.FirstName} {client.LastName}",
            }).ToList();

            return new ServiceResult<List<ClientDto>>(dtos);
        }

        public async Task<ServiceResult<ClientDto>> GetByIdAsync(Guid id)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            if (client == null)
                return new ServiceResult<ClientDto>("Client not found");

            var dto = new ClientDto
            {
                Id = client.Id,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                FullName = $"{client.FirstName} {client.LastName}",
            };

       
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client '{client.FirstName} {client.LastName}' details viewed",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<ClientDto>(dto);
        }

        public async Task<ServiceResult<List<ServicePlanDto>>> GetServicePlansAsync()
        {
            var servicePlans = await _unitOfWork.ServicePlanRepository.GetAllAsync();

            var dtos = servicePlans.Select(sp => new ServicePlanDto
            {
                Id = sp.Id,
                Name = sp.Name,
                CreatedAt = sp.CreatedAt
            }).ToList();

            return new ServiceResult<List<ServicePlanDto>>(dtos);
        }

        public async Task<ServiceResult> UpdateAsync(Guid id, UpdateClientDto dto)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(id);

            if (client == null)
                return new ServiceResult("Client not found");


            if (client.Email != dto.Email)
            {
                var existingClient = await _unitOfWork.ClientRepository.GetByEmailAsync(dto.Email);
                if (existingClient != null && existingClient.Id != id)
                    return new ServiceResult("Another client with this email already exists");
            }

            
            var oldName = $"{client.FirstName} {client.LastName}";

        
            client.FirstName = dto.FirstName;
            client.LastName = dto.LastName;
            client.Email = dto.Email;
            client.PhoneNumber = dto.PhoneNumber;
            client.Status = dto.status;
            client.HouseholdId = dto.HouseholdId;
            client.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ClientRepository.UpdateAsync(client);
            await _unitOfWork.CommitChangesAsync();

      
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client '{oldName}' updated. New name: {client.FirstName} {client.LastName}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }

        public async Task<ServiceResult<ClientDto>> GetByUserIdAsync(Guid userId)
        {
            try
            {

                var client = await _unitOfWork.ClientRepository.GetByUserIdAsync(userId);

                if (client == null)
                    return new ServiceResult<ClientDto>($"No client found for this user");

                // تبدیل به DTO
                var dto = new ClientDto
                {
                    Id = client.Id,
                    FullName = $"{client.FirstName} {client.LastName}",
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,   

                };

                // ثبت لاگ (اختیاری)
                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Client retrieved by UserId: {userId}",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult<ClientDto>(dto);
            }
            catch (Exception ex)
            {
                return new ServiceResult<ClientDto>($"Error getting client by user ID: {ex.Message}");
            }
        }
    }
}
