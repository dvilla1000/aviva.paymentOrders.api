
using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aviva.PaymentOrders.DataInfrastructure.Data
{
    public class InMemoryContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Use an in-memory database for testing
            options.UseInMemoryDatabase("avivaDb");
            options.EnableSensitiveDataLogging(); // Enable sensitive data logging for debugging purposes
            // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Use NoTracking for better performance in read-only scenarios
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentOrder> orders { get; set; }

        public DbSet<PaymentOrderDetail> orderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Product entity
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .IsRequired();
            // modelBuilder.Entity<Product>().Navigation(e => e.PaymentOrderDetails).AutoInclude();

            // Configure the PaymentOrder entity
            modelBuilder.Entity<PaymentOrder>()
                .HasKey(po => po.Id);
            modelBuilder.Entity<PaymentOrder>()
                .Property(po => po.PaymentMethod)
                .IsRequired();

            modelBuilder.Entity<PaymentOrder>()
                .Property(po => po.Status)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<PaymentOrder>().Navigation(e => e.PaymentOrderDetails).AutoInclude();

            // Configure the PaymentOrderDetail entity
            modelBuilder.Entity<PaymentOrderDetail>()
                .HasKey(pod => pod.Id);
            // .HasKey(pod => new { pod.PaymentOrderId, pod.ProductId });

            modelBuilder.Entity<PaymentOrderDetail>()
                .HasOne(pod => pod.PaymentOrder)
                .WithMany(po => po.PaymentOrderDetails)
                .HasForeignKey(pod => pod.PaymentOrderId);
            modelBuilder.Entity<PaymentOrderDetail>()
                .HasOne(pod => pod.Product)
                .WithMany(p => p.PaymentOrderDetails)
                .HasForeignKey(pod => pod.ProductId);


            modelBuilder.Entity<PaymentOrderDetail>()
                .Property(pod => pod.Quantity)
                .IsRequired();
            modelBuilder.Entity<PaymentOrderDetail>()
                .Property(pod => pod.UnitPrice)
                .IsRequired();

            // modelBuilder.Entity<PaymentOrderDetail>().Navigation(e => e.PaymentOrder).AutoInclude();
            modelBuilder.Entity<PaymentOrderDetail>().Navigation(e => e.Product).AutoInclude();
            base.OnModelCreating(modelBuilder);
        }
    }
}