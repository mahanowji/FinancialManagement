// Application/Common/Interfaces/ICurrentUserService.cs
using System;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid GetCurrentUserId();
        string? GetCurrentUserEmail();
        string? GetCurrentUserRole();
        bool IsAuthenticated();
    }
}