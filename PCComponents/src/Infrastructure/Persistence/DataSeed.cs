using Domain;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DataSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        _seedCategories(modelBuilder);
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
}