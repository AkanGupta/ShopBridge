using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopBridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Infrastructure.Mappings
{
    public class InventoryItemMapping : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(b => b.Description)
                .IsRequired(false)
                .HasColumnType("varchar(350)");

            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.Quantity)
                .IsRequired();

            builder.ToTable("InventoryItems");
        }

    }
}
