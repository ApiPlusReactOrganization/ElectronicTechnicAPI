using Application.Common.Interfaces.Repositories;
using Domain.Products;
using Domain.Products.PCComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Optional.Unsafe;

namespace Infrastructure.Persistence;

public static class DataSeederForApplicationBuilder
{
    public static async void SeedData(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var categories = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
            var manufacturers = scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
            var products = scope.ServiceProvider.GetRequiredService<IProductRepository>();

            if ((await products.GetAll(CancellationToken.None))?.Count() == 0)
            {
                // 1. GPU
                await products.Add(Product.New(
                    ProductId.New(),
                    "NVIDIA RTX 3090",
                    1499.99m,
                    "High-end gaming GPU",
                    20,
                    (await manufacturers.SearchByName(PCComponentsManufactures.Nvidia, CancellationToken.None))
                    .ValueOrDefault().Id,
                    (await categories.SearchByName(PCComponentsNames.GPU, CancellationToken.None))
                    .ValueOrDefault().Id,
                    ComponentCharacteristic.NewGpu(new GPU
                    {
                        BoostClock = 1700,
                        CoreClock = 1400,
                        FormFactor = "Dual-slot",
                        MemorySize = 24,
                        MemoryType = "GDDR6X",
                        Model = "RTX 3090"
                    })), CancellationToken.None);

                // 2. CPU
                await products.Add(Product.New(
                    ProductId.New(),
                    "Intel Core i9-13900K",
                    699.99m,
                    "High-performance CPU for gamers and creators",
                    30,
                    (await manufacturers.SearchByName(PCComponentsManufactures.Intel, CancellationToken.None))
                    .ValueOrDefault().Id,
                    (await categories.SearchByName(PCComponentsNames.CPU, CancellationToken.None))
                    .ValueOrDefault().Id,
                    ComponentCharacteristic.NewCpu(new CPU
                    {
                        Model = "Core i9-13900K",
                        Cores = 24,
                        Threads = 32,
                        BaseClock = 3.0m,
                        BoostClock = 5.8m,
                        Socket = "LGA 1700"
                    })), CancellationToken.None);

                // 3. RAM
                await products.Add(Product.New(
                    ProductId.New(),
                    "Corsair Vengeance DDR5 32GB",
                    299.99m,
                    "High-speed DDR5 memory module",
                    50,
                    (await manufacturers.SearchByName(PCComponentsManufactures.Corsair, CancellationToken.None))
                    .ValueOrDefault().Id,
                    (await categories.SearchByName(PCComponentsNames.RAM, CancellationToken.None))
                    .ValueOrDefault().Id,
                    ComponentCharacteristic.NewRam(new RAM
                    {
                        MemoryAmount = 32,
                        MemorySpeed = 5600,
                        MemoryType = "DDR5",
                        FormFactor = "DIMM",
                        Voltage = 1.1f,
                        MemoryBandwidth = 44.8f
                    })), CancellationToken.None);

                // 4. SSD
                await products.Add(Product.New(
                    ProductId.New(),
                    "Samsung 980 Pro 2TB",
                    249.99m,
                    "High-performance PCIe 4.0 NVMe SSD",
                    40,
                    (await manufacturers.SearchByName(PCComponentsManufactures.Seagate, CancellationToken.None))
                    .ValueOrDefault().Id,
                    (await categories.SearchByName(PCComponentsNames.SSD, CancellationToken.None))
                    .ValueOrDefault().Id,
                    ComponentCharacteristic.NewSSD(new SSD
                    {
                        MemoryAmount = 2000,
                        FormFactor = "M.2",
                        ReadSpeed = 7000,
                        WriteSpeed = 5100,
                        MaxTBW = 1200
                    })), CancellationToken.None);
            }
        }
    }
}