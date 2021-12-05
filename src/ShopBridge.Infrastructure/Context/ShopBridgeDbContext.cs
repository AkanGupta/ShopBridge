
using Microsoft.EntityFrameworkCore;
using ShopBridge.Domain.Models;
using System.Linq;

namespace ShopBridge.Infrastructure.Context
{
    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext(DbContextOptions options) : base(options) { }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(150)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopBridgeDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
