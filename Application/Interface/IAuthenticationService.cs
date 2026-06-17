using Application.Common.Dto;
using Domain.Abstractions;

public interface IAuthenticationService
{
    Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto dto);

    Task<ServiceResult> ChangePasswordAsync(
        Guid userId, ChangePasswordRequestDto changePasswordRequestDto);


}