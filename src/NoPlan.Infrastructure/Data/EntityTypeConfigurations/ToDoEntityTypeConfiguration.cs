using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data.EntityTypeConfigurations;

public class ToDoEntityTypeConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.ToContainer("todos");
        builder.HasNoDiscriminator();
        builder.HasPartitionKey(e => e.Id);
        builder
            .Property(e => e.ETag)
            .IsETagConcurrency();
    }
}
