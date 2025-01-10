using System.Reflection;
using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Transaction> Transactions { get; init; }
    public DbSet<TransactionMerchant> TransactionMerchants { get; init; }
    public DbSet<TransactionItem> TransactionItems { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}