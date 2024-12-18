using Microsoft.EntityFrameworkCore;
using McShopAPI.Models;
using System.Collections.Generic;

namespace McShopAPI.Data
{
    public class MusicShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        public MusicShopDbContext(DbContextOptions<MusicShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Указание составного первичного ключа для ProductSupplier
            modelBuilder.Entity<ProductSupplier>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });

            // Дополнительные настройки, если нужно
        }
    }

}