using Application.Services.HashPasswordService;
using Domain.Authentications;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using Domain.Carts;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products.PCComponents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optional;

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
        var adminRole = Role.New(AuthSettings.AdminRole);
        var userRole = Role.New(AuthSettings.UserRole);
    
        var adminId = UserId.New();
        var userId = UserId.New();
    
        var admin = User.New(adminId, "admin@example.com", "admin", hashPasswordService.HashPassword("123456"));
        var user = User.New(userId, "user@example.com", "user", hashPasswordService.HashPassword("123456"));
    
        var adminCart = Cart.New(CartId.New(), adminId);
        var userCart = Cart.New(CartId.New(), userId);
    
        modelBuilder.Entity<User>().HasData(admin, user);
    
        modelBuilder.Entity<Cart>().HasData(adminCart, userCart);
    
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.HasData(
                new { UsersId = admin.Id, RolesId = adminRole.Id },
                new { UsersId = user.Id, RolesId = userRole.Id }
            ));
    }
}