using Domain.ProductMaterials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductMaterialConfiguration : IEntityTypeConfiguration<ProductMaterial>
    {
        public void Configure(EntityTypeBuilder<ProductMaterial> builder)
        {
            builder.HasKey(pm => pm.Id);
            builder.Property(pm => pm.Id).HasConversion(pm => pm.Value, x => new ProductMaterialId(x));
            builder.Property(pm => pm.Name).IsRequired().HasMaxLength(100);
        }
    }
}