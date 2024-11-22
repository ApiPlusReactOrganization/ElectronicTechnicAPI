using Application.Common.Interfaces.Repositories;
using Application.Services.HashPasswordService;
using Domain.Authentications;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Optional;
using Optional.Unsafe;

namespace Infrastructure.Persistence;

public static class DataSeed
{
    public static void Seed(ModelBuilder modelBuilder, IHashPasswordService hashPasswordService)
    {
        _seedCategories(modelBuilder);
        _seedManufactures(modelBuilder);
        _seedRoles(modelBuilder);
        _seedUsers(modelBuilder, hashPasswordService);
    }

    private static void _seedCategories(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>();

        foreach (var category in PCComponentsNames.ListOfComponents)
        {
            categories.Add(Category.New(CategoryId.New(), category));
        }

        modelBuilder.Entity<Category>()
            .HasData(categories);
    }

    private static void _seedManufactures(ModelBuilder modelBuilder)
    {
        var manufacturers = new List<Manufacturer>();

        foreach (var manufacturer in PCComponentsManufactures.ListOfManufacturers)
        {
            manufacturers.Add(Manufacturer.New(ManufacturerId.New(), manufacturer));
        }

        modelBuilder.Entity<Manufacturer>()
            .HasData(manufacturers);
    }

    private static void _seedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<Role>();

        foreach (var role in AuthSettings.ListOfRoles)
        {
            roles.Add(Role.New(role));
        }

        modelBuilder.Entity<Role>()
            .HasData(roles);
    }

    private static void _seedUsers(ModelBuilder modelBuilder, IHashPasswordService hashPasswordService)
    {
        var users = new List<User>();

        var adminRole = Role.New(AuthSettings.AdminRole);
        var userRole = Role.New(AuthSettings.UserRole);

        var admin = User.New(UserId.New(), "admin@example.com", "admin", hashPasswordService.HashPassword("123456"));
        var user = User.New(UserId.New(), "user@example.com", "user", hashPasswordService.HashPassword("123456"));

        users.Add(admin);
        users.Add(user);

        modelBuilder.Entity<User>().HasData(users);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.HasData(
                new { UsersId = admin.Id, RolesId = adminRole.Id },
                new { UsersId = user.Id, RolesId = userRole.Id }
            ));
    }
}

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