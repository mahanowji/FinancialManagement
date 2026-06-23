using Application.Common.Interfaces;
using Application.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceCollector
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();

            return services;



        }
    }

}
