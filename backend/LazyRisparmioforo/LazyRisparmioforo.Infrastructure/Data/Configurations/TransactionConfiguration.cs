using LazyRisparmioforo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LazyRisparmioforo.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
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
        
        builder.Property(e => e.Description)
            .HasMaxLength(4000);
    }
}