using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistants.EntityConfigs
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.Property(x => x.ClientId)
                .IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(c => c.Tasks)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ClientId);

            builder.HasIndex(x => x.DueDate);

            builder.HasIndex(x => x.IsCompleted);
        }
    }
}