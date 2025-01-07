using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private bool isTestingEnvironnement;

        public DbSet<Article> Articles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options, bool isTestingEnvironnement = false) : base(options)
        {
            this.isTestingEnvironnement = isTestingEnvironnement;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!isTestingEnvironnement)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Ex06_EntityFramework;Trusted_Connection=True;", options => options.MigrationsAssembly("Infrastructure"));
            }            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.Address);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Warehouse)
                .WithMany(w => w.Orders)
                .HasForeignKey(o => o.WarehouseId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Article)
                .WithMany()
                .HasForeignKey(od => od.ArticleId);

            modelBuilder.Entity<Warehouse>()
                .Property(e => e.CodeAccesMD5)
                .HasConversion(v => string.Join(";", v),
                v => v.Split(new[] { ';' }, StringSplitOptions.None).ToList());
        }
    }
}
