using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.Configurations
{
    public class NotificationEventConfiguration : IEntityTypeConfiguration<NotificationEvent>
    {
        public void Configure(EntityTypeBuilder<NotificationEvent> builder)
        {

            builder.HasKey(x => x.Id);


            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.SentAt)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .IsRequired();


            builder.HasOne(x => x.Client)
                .WithMany(c => c.NotificationEvents)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ClientId);

            builder.HasIndex(x => x.SentAt);
        }
    }
}