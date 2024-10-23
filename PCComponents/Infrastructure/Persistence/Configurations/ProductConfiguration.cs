using Domain.Products;
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

            // Налаштування зв'язку з ProductMaterials
            builder.HasMany(p => p.ProductMaterials)
                .WithMany(pm => pm.Products)
                .UsingEntity(j => j.ToTable("ProductProductMaterials"));
        }
    }
}