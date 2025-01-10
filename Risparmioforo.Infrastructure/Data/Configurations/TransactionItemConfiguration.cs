using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
{
    public void Configure(EntityTypeBuilder<TransactionItem> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Item)
            .HasMaxLength(255);      
    }
}