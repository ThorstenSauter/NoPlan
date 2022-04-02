using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data.EntityTypeConfigurations;

public sealed class ToDoEntityTypeConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder
            .ToContainer("todos")
            .HasNoDiscriminator()
            .HasPartitionKey(e => e.CreatedBy)
            .HasKey(e => e.Id);

        builder
            .Property(e => e.ETag)
            .IsETagConcurrency();
    }
}
