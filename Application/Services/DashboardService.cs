using Application.Common.Interfaces;
using Domain.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditLogService _auditLogService;

        public DashboardService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _auditLogService = auditLogService;
        }

        public async Task<ServiceResult<DashboardDto>> GetDashboardAsync()
        {
            try
            {
                var dashboard = new DashboardDto();

            
                dashboard.TotalClients = await GetTotalClientsCountAsync();

                dashboard.PendingTasks = await GetPendingTasksCountAsync();

        
                dashboard.UnpaidInvoices = await GetUnpaidInvoicesCountAsync();

  
                dashboard.RecentDocuments = await GetRecentDocumentsCountAsync();

                // ثبت لاگ
                await _auditLogService.CreateAsync(new CreateAuditLogDto
                {
                    Action = $"Dashboard viewed: TotalClients={dashboard.TotalClients}, " +
                             $"PendingTasks={dashboard.PendingTasks}, " +
                             $"UnpaidInvoices={dashboard.UnpaidInvoices}, " +
                             $"RecentDocuments={dashboard.RecentDocuments}",
                    UserId = _currentUserService.GetCurrentUserId()
                });

                return new ServiceResult<DashboardDto>(dashboard);
            }
            catch (Exception ex)
            {
                return new ServiceResult<DashboardDto>($"Error loading dashboard: {ex.Message}");
            }
        }

        #region Private Methods

        private async Task<int> GetTotalClientsCountAsync()
        {
            return await _unitOfWork.ClientRepository.GetCountAsync();
        }

        private async Task<int> GetPendingTasksCountAsync()
        {

            return 0;
        }

        private async Task<int> GetUnpaidInvoicesCountAsync()
        {
            return await _unitOfWork.InvoiceRepository.GetUnpaidCountAsync();

        }

        private async Task<int> GetRecentDocumentsCountAsync()
        {
            return await _unitOfWork.DocumentRepository.GetRecentCountAsync(30);

        }

        #endregion
    }
}