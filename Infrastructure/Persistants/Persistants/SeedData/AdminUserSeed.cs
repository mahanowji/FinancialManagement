using Dapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistants.Persistants.SeedData
{
    public static class AdminUserSeed
    {
        public static void SeedDataExtensionUsers(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            using var connection = unitOfWork.SqlConnectionFactory.CreateConnection();

            var Email = "mahanowji1380@gmail.com";

            var exists = connection.ExecuteScalar<bool>(
                "SELECT COUNT(1) FROM Users WHERE Email = @Email",
                new { Email });


            if (!exists)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "هاشم",
                    LastName = "طوبایی نژاد",
                    Email = Email.ToLower(),
                    PasswordHash = SecurityHelper.HashPassword("kazeemm_1344@#4"),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                connection.Execute(@"
                        INSERT INTO Users (Id, FirstName, Email, LastName, PasswordHash, IsDeleted, Role,CreatedAt,UpdatedAt)
                        VALUES (@Id, @FirstName, @Email, @LastName, @PasswordHash, 0, @Role,@CreatedAt,@UpdatedAt)", user);



                Console.WriteLine("✅ Admin user inserted successfully.");
            }
            else
            {
                Console.WriteLine("ℹ️ Admin user already exists.");
            }
        }
    }
}
