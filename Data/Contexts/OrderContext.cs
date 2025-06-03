using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderEntity>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.Email)
            .IsUnique();
    }
}
