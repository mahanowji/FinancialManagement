using Domain.Abstractions;

public interface IDashboardService
{
    Task<ServiceResult<DashboardDto>>
        GetDashboardAsync();
}