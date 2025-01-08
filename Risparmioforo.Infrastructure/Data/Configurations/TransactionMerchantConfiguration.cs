using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data.Configurations;

public class TransactionMerchantConfiguration : IEntityTypeConfiguration<TransactionMerchant>
{
    public void Configure(EntityTypeBuilder<TransactionMerchant> builder)
    {
        builder.HasKey(e => e.Id);
    }
}