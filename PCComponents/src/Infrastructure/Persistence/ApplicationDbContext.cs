using System.Reflection;
using Application.Services.HashPasswordService;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Orders;
using Domain.Products;
using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHashPasswordService hashPasswordService) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        DataSeed.Seed(builder, hashPasswordService);
    }
}