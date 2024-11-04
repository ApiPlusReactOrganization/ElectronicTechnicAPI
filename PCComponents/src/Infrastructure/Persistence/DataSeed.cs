using Application.Authentications.Services;
using Domain;
using Domain.Authentications;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DataSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        _seedCategories(modelBuilder);
        _seedManufactures(modelBuilder);
        _seedRoles(modelBuilder);
        _seedUsers(modelBuilder);
    }

    public static void _seedCategories(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>();

        foreach (var category in PCComponentsNames.ListOfComponents)
        {
            categories.Add(Category.New(CategoryId.New(), category));
        }

        modelBuilder.Entity<Category>()
            .HasData(categories);
    }
    
    public static void _seedManufactures(ModelBuilder modelBuilder)
    {
        var manufacturers = new List<Manufacturer>();

        foreach (var manufacturer in PCComponentsManufactures.ListOfManufacturers)
        {
            manufacturers.Add(Manufacturer.New(ManufacturerId.New(), manufacturer));
        }

        modelBuilder.Entity<Manufacturer>()
            .HasData(manufacturers);
    }

    public static void _seedRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<Role>();

        foreach (var role in AuthSettings.ListOfRoles)
        {
            roles.Add(Role.New(role));
        }

        modelBuilder.Entity<Role>()
            .HasData(roles);
    }

    public static void _seedUsers(ModelBuilder modelBuilder)
    {
        var users = new List<User>();
    
        // Define roles
        var adminRole = Role.New(AuthSettings.AdminRole);
        var userRole = Role.New(AuthSettings.UserRole);
    
        // Create users
        var admin = User.New(UserId.New(), "admin@example.com", "admin", HashPasswordService.HashPassword("123456"));
        var user = User.New(UserId.New(), "user@example.com", "user", HashPasswordService.HashPassword("123456"));

        // Add users to the list
        users.Add(admin);
        users.Add(user);

        // Seed users
        modelBuilder.Entity<User>().HasData(users);

        // Seed user roles
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.HasData(
                new { UsersId = admin.Id, RolesId = adminRole.Id },
                new { UsersId = user.Id, RolesId = userRole.Id }
            ));
    }

}