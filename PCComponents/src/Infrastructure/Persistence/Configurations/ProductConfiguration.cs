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
                productBuilder.ToJson("component characteristics");

                productBuilder.OwnsOne(x => x.Case, caseBuilder =>
                {
                    caseBuilder.Property(x => x.CoolingDescription).HasJsonPropertyName("cooling system");
                    caseBuilder.Property(x => x.NumberOfFans).HasJsonPropertyName("number of fans");
                    caseBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                    caseBuilder.Property(x => x.CompartmentDescription).HasJsonPropertyName("compartment description");
                    caseBuilder.Property(x => x.PortsDescription).HasJsonPropertyName("ports description");
                });

                productBuilder.OwnsOne(x => x.Cpu, cpuBuilder =>
                {
                    cpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    cpuBuilder.Property(x => x.Cores).HasJsonPropertyName("cores");
                    cpuBuilder.Property(x => x.Threads).HasJsonPropertyName("threads");
                    cpuBuilder.Property(x => x.BaseClock).HasJsonPropertyName("base clock")
                        .HasColumnType("decimal(5, 2)");
                    cpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boost clock")
                        .HasColumnType("decimal(5, 2)");
                    cpuBuilder.Property(x => x.Socket).HasJsonPropertyName("socket");
                });

                productBuilder.OwnsOne(x => x.Gpu, gpuBuilder =>
                {
                    gpuBuilder.Property(x => x.Model).HasJsonPropertyName("model");
                    gpuBuilder.Property(x => x.MemorySize).HasJsonPropertyName("memory size");
                    gpuBuilder.Property(x => x.MemoryType).HasJsonPropertyName("memory type");
                    gpuBuilder.Property(x => x.CoreClock).HasJsonPropertyName("core clock")
                        .HasColumnType("decimal(6, 2)");
                    gpuBuilder.Property(x => x.BoostClock).HasJsonPropertyName("boost clock")
                        .HasColumnType("decimal(6, 2)");
                    gpuBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                });

                productBuilder.OwnsOne(x => x.Motherboard, motherboardBuilder =>
                {
                    motherboardBuilder.Property(x => x.RAMDescription).HasJsonPropertyName("RAM description");
                    motherboardBuilder.Property(x => x.NetworkDescription).HasJsonPropertyName("network description");
                    motherboardBuilder.Property(x => x.PowerDescription).HasJsonPropertyName("power description");
                    motherboardBuilder.Property(x => x.AudioDescription).HasJsonPropertyName("audio description");
                    motherboardBuilder.Property(x => x.ExternalConnectorsDescription)
                        .HasJsonPropertyName("external connectors description");
                    motherboardBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                    motherboardBuilder.Property(x => x.Socket).HasJsonPropertyName("socket");

                });

                productBuilder.OwnsOne(x => x.Psu, psuBuilder =>
                {
                    psuBuilder.Property(x => x.PowerCapacity).HasJsonPropertyName("power capacity");
                    psuBuilder.Property(x => x.InputVoltageRange).HasJsonPropertyName("input voltage range");
                    psuBuilder.Property(x => x.FanTypeAndSize).HasJsonPropertyName("fan type and size");
                    psuBuilder.Property(x => x.Protections).HasJsonPropertyName("protections");
                    psuBuilder.Property(x => x.Connectors).HasJsonPropertyName("connectors");
                });

                productBuilder.OwnsOne(x => x.Ram, psuBuilder =>
                {
                    psuBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memory amount");
                    psuBuilder.Property(x=> x.MemorySpeed).HasJsonPropertyName("memory speed");
                    psuBuilder.Property(x => x.MemoryType).HasJsonPropertyName("memory type");
                    psuBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                    psuBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    psuBuilder.Property(x => x.MemoryBandwidth).HasJsonPropertyName("memory bandwidth");
                });

                productBuilder.OwnsOne(x => x.Cooler, coolerBuilder =>
                {
                    coolerBuilder.Property(x => x.Material).HasJsonPropertyName("material");
                    coolerBuilder.Property(x => x.Fanspeed).HasJsonPropertyName("fan speed");
                    coolerBuilder.Property(x => x.FanAmount).HasJsonPropertyName("fan amount");
                    coolerBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    coolerBuilder.Property(x => x.MaxTDP).HasJsonPropertyName("max tdp");
                    coolerBuilder.Property(x => x.FanSupply).HasJsonPropertyName("fan supply");
                    coolerBuilder.Property(x => x.Sockets).HasJsonPropertyName("sockets");
                    
                });

                productBuilder.OwnsOne(x => x.Hdd, hddBuilder =>
                {
                    hddBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memory amount");
                    hddBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                    hddBuilder.Property(x => x.Voltage).HasJsonPropertyName("voltage");
                    hddBuilder.Property(x => x.ReadSpeed).HasJsonPropertyName("read speed");
                    hddBuilder.Property(x => x.WriteSpeed).HasJsonPropertyName("write speed");
                });
                
                productBuilder.OwnsOne(x => x.Ssd, sddBuilder =>
                {
                    sddBuilder.Property(x => x.MemoryAmount).HasJsonPropertyName("memory amount");
                    sddBuilder.Property(x => x.FormFactor).HasJsonPropertyName("form factor");
                    sddBuilder.Property(x => x.ReadSpeed).HasJsonPropertyName("read speed");
                    sddBuilder.Property(x => x.WriteSpeed).HasJsonPropertyName("write speed");
                    sddBuilder.Property(x => x.MaxTBW).HasJsonPropertyName("max tbw");
                });
            });
        }
    }
}