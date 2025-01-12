using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Account;
using Risparmioforo.Domain.Category;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();      
    }
}