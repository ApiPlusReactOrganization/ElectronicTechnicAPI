﻿using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasConversion(p => p.Value, x => new ProductId(x));
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.StockQuantity).IsRequired();
            
            builder.HasOne(p => p.Manufacturer)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            
            builder.OwnsOne(x => x.ComponentCharacteristic, productBuilder =>
            {
                productBuilder.ToJson("component characteristic");

                productBuilder.Property(x => x.Type).HasJsonPropertyName("type");

                productBuilder.OwnsOne(x => x.Case, caseBuilder =>
                {
                    caseBuilder.Property(x => x.CoolingSystem).HasJsonPropertyName("cooling system");
                    caseBuilder.Property(x => x.NumberOfFans).HasJsonPropertyName("number of fans");
                        
                    caseBuilder.Property(x => x.FormFactors).HasJsonPropertyName("form factors");
                });
                
                productBuilder.OwnsOne(x => x.Processor, caseBuilder =>
                {
                    caseBuilder.Property(x => x.Series).HasJsonPropertyName("cooling system");
                    caseBuilder.Property(x => x.NumberOfСores).HasJsonPropertyName("2");
                    caseBuilder.Property(x => x.NumberOfStreams).HasJsonPropertyName("12");
                });
            });
        }
    }
}