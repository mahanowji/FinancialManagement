using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.EntityConfigs
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {

            // Primary Key
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Amount)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();


            builder.HasOne(x => x.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => x.InvoiceNumber)
                .IsUnique();

            builder.HasIndex(x => x.ClientId);
        }
    }
}