using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.Configurations
{
    public class PaymentRecordConfiguration : IEntityTypeConfiguration<PaymentRecord>
    {
        public void Configure(EntityTypeBuilder<PaymentRecord> builder)
        {

            builder.HasKey(x => x.Id);


            builder.Property(x => x.Amount)
                .IsRequired();


            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.PaidAt)
                .IsRequired(false);

            builder.Property(x => x.InvoiceId)
                .IsRequired();

            builder.HasOne(x => x.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.InvoiceId);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => x.PaidAt);
        }
    }
}