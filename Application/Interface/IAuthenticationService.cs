using Domain.Abstractions;

public interface IAuthenticationService
{
    Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto dto);

    Task<ServiceResult> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword);

    Task<ServiceResult> LogoutAsync(Guid userId);
}