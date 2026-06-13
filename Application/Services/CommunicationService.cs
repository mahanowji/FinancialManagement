using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class CommunicationService : ICommunicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly ICurrentUserService _currentUserService;

        public CommunicationService(
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateCommunicationDto dto)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult<Guid>("Client not found");

            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(currentUserId);

            if (currentUser == null)
                return new ServiceResult<Guid>("Current user not found");

            var communication = new Communication
            {
                Id = Guid.NewGuid(),
                ClientId = dto.ClientId,
                Type = dto.Type,
                Description = dto.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,   
                OccurredAt = DateTime.Now,  
            };

            await _unitOfWork.CommunicationRepository.AddAsync(communication);
            await _unitOfWork.CommitChangesAsync();

       
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Communication ({communication.Type}) added for client '{client.FirstName} {client.LastName}'          ",
                UserId = currentUserId
            });

            return new ServiceResult<Guid>(communication.Id);
        }

        public async Task<ServiceResult<List<CommunicationDto>>> GetClientHistoryAsync(Guid clientId)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
            if (client == null)
                return new ServiceResult<List<CommunicationDto>>("Client not found");

            // گرفتن همه ارتباطات کلاینت
            var communications = await _unitOfWork.CommunicationRepository
                .GetByClientIdAsync(clientId);

            var dtos = communications.Select(c => new CommunicationDto
            {
                Id = c.Id,
                ClientId = c.ClientId,
                Type = c.Type,
                Description= c.Description,
                OccurredAt= c.OccurredAt,

            }).ToList();

            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client '{client.FirstName} {client.LastName}' communication history viewed ({dtos.Count} records)",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<List<CommunicationDto>>(dtos);
        }
    }
}
