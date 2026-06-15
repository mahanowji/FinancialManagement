using Application.Common.Dto;
using Application.Common.Interfaces;
using Application.Utilities;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditLogService _auditLogService;

    public UserService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateUserDto dto)
    {
        try
        {

            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return new ServiceResult<Guid>($"User with email '{dto.Email}' already exists");


            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };


            user.PasswordHash = SecurityHelper.HashPassword(dto.Password);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitChangesAsync();


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"User '{user.Email}' created with role {user.Role}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(user.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating user: {ex.Message}");
        }
    }

    public async Task<ServiceResult<UserDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return new ServiceResult<UserDto>($"User not found");

            var dto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"User '{user.Email}' details viewed",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<UserDto>(dto);
        }
        catch (Exception ex)
        {
            return new ServiceResult<UserDto>($"Error getting user: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<UserDto>>> GetAllAsync()
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            var dtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            }).ToList();

            return new ServiceResult<List<UserDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<UserDto>>($"Error getting users: {ex.Message}");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return new ServiceResult($"User not found");

            if (user.Role == UserRole.Admin && user.Email == "admin@example.com")
                return new ServiceResult($"Cannot delete main admin user");

            var clientWithThisUser = await _unitOfWork.ClientRepository.GetAllClientUserAsync(id);
            if (clientWithThisUser != null)
                return new ServiceResult($"Cannot delete user because they are linked to a client");

            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.CommitChangesAsync();


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"User '{user.Email}' deleted",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }
        catch (Exception ex)
        {
            return new ServiceResult($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Guid>> CreateHouseholdAsync(CreateHouseholdDto dto)
    {
        try
        {
            var household = new Household
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.HouseholdRepository.AddAsync(household);
            await _unitOfWork.CommitChangesAsync();

            // ثبت لاگ
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Household '{household.Name}' created",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(household.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating household: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<HouseholdDto>>> GetHouseholdsAsync()
    {
        try
        {
            var households = await _unitOfWork.HouseholdRepository.GetAllAsync();

            var dtos = households.Select(h => new HouseholdDto
            {
                Id = h.Id,
                Name = h.Name,
                CreatedAt = h.CreatedAt
            }).ToList();

            return new ServiceResult<List<HouseholdDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<HouseholdDto>>($"Error getting households: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Guid>> CreateClientUserAsync(CreateClientUserDto dto)
    {
        try
        {

            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult<Guid>($"Client not found");


            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return new ServiceResult<Guid>($"User with email '{dto.Email}' already exists");

            var existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(client.Email);
            if (existingUserByEmail != null)
                return new ServiceResult<Guid>($"Client's email '{client.Email}' is already used by another user account");

     
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = UserRole.Client,
                CreatedAt = DateTime.UtcNow
            };


            user.PasswordHash = SecurityHelper.HashPassword(dto.Password);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitChangesAsync();


            client.AdvisorId = user.Id;
            await _unitOfWork.ClientRepository.UpdateAsync(client);
            await _unitOfWork.CommitChangesAsync();


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Client user account created for '{client.FirstName} {client.LastName}' with email '{dto.Email}'",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(user.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating client user: {ex.Message}");
        }
    }
}