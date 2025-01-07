using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class PriceConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.ValueDate)
            .IsRequired();      
        
        builder.Property(e => e.RegistrationDate)
            .IsRequired();
        
        builder.Property(e => e.Amount)
            .IsRequired();
    }
}