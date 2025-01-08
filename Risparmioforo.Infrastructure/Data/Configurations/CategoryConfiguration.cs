using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Category;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();      
    }
}