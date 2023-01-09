using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoPlan.Infrastructure.Data.Models;

namespace NoPlan.Infrastructure.Data.EntityTypeConfigurations;

internal sealed class ToDoEntityTypeConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder) =>
        builder.OwnsMany(t => t.Tags);
}
