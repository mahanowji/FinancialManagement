using Application.Common;
using Application.Utilities;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new ServiceResult<LoginResultDto>(MessageResponse.UserNotFound);
            }

            var isValidPassword = SecurityHelper.VerifyPassword(currentPassword, user.PasswordHash);

            if (!isValidPassword)
            {
                return new ServiceResult<LoginResultDto>(MessageResponse.loginfailed);
            }

            user.PasswordHash =SecurityHelper.HashPassword(newPassword);

            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.CommitChangesAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto dto)
        {
            var user = await unitOfWork.UserRepository.GetByEmailAsync(dto.Email);

            if (user is null)
            {
                return new ServiceResult<LoginResultDto>(MessageResponse.UserNotFound);
            }

            var isValidPassword = SecurityHelper.VerifyPassword(dto.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return new ServiceResult<LoginResultDto>(MessageResponse.loginfailed);
            }


            var token = unitOfWork.JwtRepository.GenerateToken(user);

            var result = new LoginResultDto
            {
                Token = token,
                UserId = user.Id,
                Role = user.Role.ToString()
            };


            return new ServiceResult<LoginResultDto>(result);
        }

        public Task<ServiceResult> LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
