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

                // productBuilder.Property(x => x.Category).HasJsonPropertyName("category");

                productBuilder.OwnsOne(x => x.Case, caseBuilder =>
                {
                    caseBuilder.Property(x => x.CoolingSystem).HasJsonPropertyName("cooling system");
                    caseBuilder.Property(x => x.NumberOfFans).HasJsonPropertyName("number of fans");
                    caseBuilder.Property(x => x.FormFactors).HasJsonPropertyName("form factors");
                });

                productBuilder.OwnsOne(x => x.Cpu, cpuBuilder =>
                {
                    cpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    cpuBuilder.Property(x => x.Cores).HasJsonPropertyName("cores");
                    cpuBuilder.Property(x => x.Threads).HasJsonPropertyName("threads");
                    cpuBuilder.Property(x => x.BaseClock).HasJsonPropertyName("base clock").HasColumnType("decimal(5, 2)");
                    cpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boost clock").HasColumnType("decimal(5, 2)");
                });

                productBuilder.OwnsOne(x => x.Gpu, gpuBuilder =>
                {
                    gpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    gpuBuilder.Property(x => x.MemorySize).HasJsonPropertyName("memory size");
                    gpuBuilder.Property(x => x.MemoryType).HasJsonPropertyName("memory type");
                    gpuBuilder.Property(x => x.CoreClock).HasJsonPropertyName("core clock").HasColumnType("decimal(6, 2)");
                    gpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boost clock").HasColumnType("decimal(6, 2)");
                });
            });
        }
    }
}