using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.EntityConfigs
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Status)
                .IsRequired();

            // =========================
            // Relationships
            // =========================

            builder.HasOne(x => x.Advisor)
                .WithMany(x=>x.Clients)
                .HasForeignKey(x => x.AdvisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Household)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.HouseholdId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(x => x.ServicePlan)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.ServicePlanId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}