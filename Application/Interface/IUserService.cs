using Domain.Abstractions;

public interface IUserService
{
    Task<ServiceResult<Guid>> CreateAsync(CreateUserDto dto);

    Task<ServiceResult<UserDto>> GetByIdAsync(Guid id);

    Task<ServiceResult<List<UserDto>>> GetAllAsync();

    Task<ServiceResult> DeleteAsync(Guid id);

    Task<ServiceResult<Guid>> CreateHouseholdAsync(CreateHouseholdDto dto);

    Task<ServiceResult<List<HouseholdDto>>> GetHouseholdsAsync();
}