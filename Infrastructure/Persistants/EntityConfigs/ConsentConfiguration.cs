using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.Configurations
{
    public class ConsentConfiguration : IEntityTypeConfiguration<Consent>
    {
        public void Configure(EntityTypeBuilder<Consent> builder)
        {

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Accepted)
                .IsRequired();

            builder.Property(x => x.AcceptedAt)
                .IsRequired(false);

            builder.Property(x => x.ClientId)
                .IsRequired();


            builder.HasOne(x => x.Client)
                .WithMany(c => c.Consents)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ClientId);
        }
    }
}