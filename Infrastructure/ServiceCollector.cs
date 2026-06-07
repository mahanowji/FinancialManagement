using Domain.Abstractions;
using Infrastructure.Persistants;
using Infrastructure.Persistants.Persistants.Datas;
using Infrastructure.Persistants.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollector
    {

        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer") ??
                    throw new ArgumentNullException(nameof(configuration));

            Console.WriteLine($"Using Connection String: {connectionString}");


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<ISqlConnectionFactory>(x => new SqlConnectionFactory(connectionString));

        }


        public static void RegisterRepositories(this IServiceCollection services)
        {

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICommunicationRepository, CommunicationRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();


        }
    }
}
