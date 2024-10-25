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
            
            builder.HasOne(p => p.Manufacturer)
                .WithMany()
                .HasForeignKey(p => p.ManufacturerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Конфігурація зв'язку з ProductMaterials
            builder.HasMany(p => p.ProductMaterials)
                .WithMany()
                .UsingEntity(j => j.ToTable("pmk_products_product_material"));
            
            // Конфігурація зв'язку з FormFactors
            builder.HasMany(p => p.FormFactors)
                .WithMany()
                .UsingEntity(j => j.ToTable("ffk_products_form_factors_id"));
            
            builder.OwnsOne(x => x.ComponentCharacteristic, productBuilder =>
            {
                productBuilder.ToJson("component characteristic");

                productBuilder.Property(x => x.Type).HasJsonPropertyName("type");

                productBuilder.OwnsOne(x => x.Case, caseBuilder =>
                {
                    caseBuilder.Property(x => x.CoolingSystem).HasJsonPropertyName("cooling system");
                    caseBuilder.Property(x => x.NumberOfFans).HasJsonPropertyName("number of fans");
                        
                    caseBuilder.OwnsMany(x => x.FormFactors, formFactorsBuilder =>
                    {
                        formFactorsBuilder.Property(x => x.Name).HasJsonPropertyName("name");
                    });
                });

                /*productBuilder.OwnsOne(x => x.TechnicalEquipment, technicalEquipment =>
                {
                    technicalEquipment.Property(x => x.Name).HasJsonPropertyName("name");
                    technicalEquipment.Property(x => x.Weight).HasJsonPropertyName("weight");

                    technicalEquipment.OwnsMany(x => x.Parts, partsBuilder =>
                    {
                        partsBuilder.Property(x => x.Name).HasJsonPropertyName("name");
                    });
                });*/
            });
        }
    }
}