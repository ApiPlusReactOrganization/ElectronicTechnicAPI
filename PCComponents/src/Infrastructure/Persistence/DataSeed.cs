using Domain;
using Domain.Auth;
using Domain.Auth.Roles;
using Domain.Categories;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DataSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        _seedCategories(modelBuilder);
        
        _seedRoles(modelBuilder);
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
}