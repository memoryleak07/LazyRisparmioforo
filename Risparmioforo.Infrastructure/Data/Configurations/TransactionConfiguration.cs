using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class PriceConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // builder.ToTable("Transactions");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Date)
            .IsRequired();
        
        builder.Property(e => e.Amount)
            .IsRequired();
    }
}