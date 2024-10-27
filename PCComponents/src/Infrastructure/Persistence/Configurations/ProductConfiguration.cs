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
                
                productBuilder.OwnsOne(x => x.CPU, cpuBuilder =>
                {
                    cpuBuilder.Property(x => x.Socket).HasJsonPropertyName("socket");
                    cpuBuilder.Property(x => x.NumberOfCores).HasJsonPropertyName("number of cores");
                    cpuBuilder.Property(x => x.NumberOfThreads).HasJsonPropertyName("number of threads");
                    cpuBuilder.Property(x => x.BaseClock).HasJsonPropertyName("base clock");
                });
                
                productBuilder.OwnsOne(x => x.Motherboard, motherboard =>
                {
                    motherboard.Property(x => x.Socket).HasJsonPropertyName("socket");
                    motherboard.Property(x => x.FormFactors).HasJsonPropertyName("form factors");
                    motherboard.Property(x => x.RAMDescription).HasJsonPropertyName("RAM description");
                    motherboard.Property(x => x.NetworkDescription).HasJsonPropertyName("network description");
                    motherboard.Property(x => x.PowerDescription).HasJsonPropertyName("power description");
                    motherboard.Property(x => x.AudioDescription).HasJsonPropertyName("audio description");
                    motherboard.Property(x => x.ExternalConnectorsDescription).HasJsonPropertyName("external onnectors description");
                });
                
                productBuilder.OwnsOne(x => x.PSU, psuBuilder =>
                {
                    psuBuilder.Property(x => x.PowerCapacity).HasJsonPropertyName("power capacity");
                    psuBuilder.Property(x => x.InputVoltageRange).HasJsonPropertyName("input voltage range");
                    psuBuilder.Property(x => x.FanTypeAndSize).HasJsonPropertyName("fan type and Size");
                    psuBuilder.Property(x => x.Protections).HasJsonPropertyName("protections");
                    psuBuilder.Property(x => x.Connectors).HasJsonPropertyName("connectors");
                });
            });
        }
    }
}