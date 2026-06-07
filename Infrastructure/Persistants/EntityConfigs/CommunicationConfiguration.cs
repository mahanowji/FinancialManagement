using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.EntityConfigs
{
    public class CommunicationConfiguration : IEntityTypeConfiguration<Communication>
    {
        public void Configure(EntityTypeBuilder<Communication> builder)
        {


            builder.HasKey(x => x.Id);


            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.OccurredAt)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .IsRequired();


            builder.HasOne(x => x.Client)
                .WithMany(c => c.Communications)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => x.ClientId);
        }
    }
}