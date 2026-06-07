using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.EntityConfigs
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {


            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.ClientId)
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(c => c.Documents)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(x => x.Category)
                .WithMany(c => c.Documents)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.CategoryId);
        }
    }
}