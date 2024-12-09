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
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.OwnsOne(x => x.ComponentCharacteristic, productBuilder =>
            {
                productBuilder.ToJson("componentCharacteristics");

                productBuilder.OwnsOne(x => x.Case, caseBuilder =>
                {
                    caseBuilder.Property(x => x.CoolingDescription).HasJsonPropertyName("coolingSystem");
                    caseBuilder.Property(x => x.NumberOfFans).HasJsonPropertyName("numberOfFans");
                    caseBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                    caseBuilder.Property(x => x.CompartmentDescription).HasJsonPropertyName("compartmentDescription");
                    caseBuilder.Property(x => x.PortsDescription).HasJsonPropertyName("portsDescription");
                });

                productBuilder.OwnsOne(x => x.Cpu, cpuBuilder =>
                {
                    cpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    cpuBuilder.Property(x => x.Cores).HasJsonPropertyName("cores");
                    cpuBuilder.Property(x => x.Threads).HasJsonPropertyName("threads");
                    cpuBuilder.Property(x => x.BaseClock).HasJsonPropertyName("baseClock")
                        .HasColumnType("decimal(5, 2)");
                    cpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boostClock")
                        .HasColumnType("decimal(5, 2)");
                    cpuBuilder.Property(x => x.Socket).HasJsonPropertyName("socket");
                });

                productBuilder.OwnsOne(x => x.Gpu, gpuBuilder =>
                {
                    gpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    gpuBuilder.Property(x => x.MemorySize).HasJsonPropertyName("memorySize");
                    gpuBuilder.Property(x => x.MemoryType).HasJsonPropertyName("memoryType");
                    gpuBuilder.Property(x => x.CoreClock).HasJsonPropertyName("coreClock")
                        .HasColumnType("decimal(6, 2)");
                    gpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boostClock")
                        .HasColumnType("decimal(6, 2)");
                    gpuBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                });

                productBuilder.OwnsOne(x => x.Motherboard, motherboardBuilder =>
                {
                    motherboardBuilder.Property(x => x.RAMDescription).HasJsonPropertyName("ramDescription");
                    motherboardBuilder.Property(x => x.NetworkDescription).HasJsonPropertyName("networkDescription");
                    motherboardBuilder.Property(x => x.PowerDescription).HasJsonPropertyName("powerDescription");
                    motherboardBuilder.Property(x => x.AudioDescription).HasJsonPropertyName("audioDescription");
                    motherboardBuilder.Property(x => x.ExternalConnectorsDescription)
                        .HasJsonPropertyName("externalConnectorsDescription");
                    motherboardBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                    motherboardBuilder.Property(x => x.Socket).HasJsonPropertyName("socket");
                });

                productBuilder.OwnsOne(x => x.Psu, psuBuilder =>
                {
                    psuBuilder.Property(x => x.PowerCapacity).HasJsonPropertyName("powerCapacity");
                    psuBuilder.Property(x => x.InputVoltageRange).HasJsonPropertyName("inputVoltageRange");
                    psuBuilder.Property(x => x.FanTypeAndSize).HasJsonPropertyName("fanTypeAndSize");
                    psuBuilder.Property(x => x.Protections).HasJsonPropertyName("protections");
                    psuBuilder.Property(x => x.Connectors).HasJsonPropertyName("connectors");
                });

                productBuilder.OwnsOne(x => x.Ram, ramBuilder =>
                {
                    ramBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memoryAmount");
                    ramBuilder.Property(x => x.MemorySpeed).HasJsonPropertyName("memorySpeed");
                    ramBuilder.Property(x => x.MemoryType).HasJsonPropertyName("memoryType");
                    ramBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                    ramBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    ramBuilder.Property(x => x.MemoryBandwidth).HasJsonPropertyName("memoryBandwidth");
                });

                productBuilder.OwnsOne(x => x.Cooler, coolerBuilder =>
                {
                    coolerBuilder.Property(x => x.Material).HasJsonPropertyName("material");
                    coolerBuilder.Property(x => x.Fanspeed).HasJsonPropertyName("fanSpeed");
                    coolerBuilder.Property(x => x.FanAmount).HasJsonPropertyName("fanAmount");
                    coolerBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    coolerBuilder.Property(x => x.MaxTDP).HasJsonPropertyName("maxTdp");
                    coolerBuilder.Property(x => x.FanSupply).HasJsonPropertyName("fanSupply");
                    // coolerBuilder.Property(x => x.Sockets).HasJsonPropertyName("sockets");
                });

                productBuilder.OwnsOne(x => x.Hdd, hddBuilder =>
                {
                    hddBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memoryAmount");
                    hddBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                    hddBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    hddBuilder.Property(x => x.ReadSpeed).HasJsonPropertyName("readSpeed");
                    hddBuilder.Property(x => x.WriteSpeed).HasJsonPropertyName("writeSpeed");
                });

                productBuilder.OwnsOne(x => x.Ssd, ssdBuilder =>
                {
                    ssdBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memoryAmount");
                    ssdBuilder.Property(x => x.FormFactor).HasJsonPropertyName("formFactor");
                    ssdBuilder.Property(x => x.ReadSpeed).HasJsonPropertyName("readSpeed");
                    ssdBuilder.Property(x => x.WriteSpeed).HasJsonPropertyName("writeSpeed");
                    ssdBuilder.Property(x => x.MaxTBW).HasJsonPropertyName("maxTbw");
                });
            });
        }
    }
}