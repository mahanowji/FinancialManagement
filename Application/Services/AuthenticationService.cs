using Application.Common;
using Application.Common.Dto;
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

        public async Task<ServiceResult> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new ServiceResult<LoginResultDto>("user is not found");
            }

            var isValidPassword = SecurityHelper.VerifyPassword(changePasswordRequestDto.CurrentPassword, user.PasswordHash);

            if (!isValidPassword)
            {
                return new ServiceResult<LoginResultDto>("log failed");
            }

            user.PasswordHash =SecurityHelper.HashPassword(changePasswordRequestDto.NewPassword);

            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.CommitChangesAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto dto)
        {
            var user = await unitOfWork.UserRepository.GetByEmailAsync(dto.Email);

            if (user is null)
            {
                return new ServiceResult<LoginResultDto>("user not found");
            }

            var isValidPassword = SecurityHelper.VerifyPassword(dto.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return new ServiceResult<LoginResultDto>("login failed");
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

    }
}
