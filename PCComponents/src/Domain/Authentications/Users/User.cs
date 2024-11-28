using Domain.Authentications.Roles;
using Domain.Carts;
using Domain.Orders;
using Domain.Products;

namespace Domain.Authentications.Users;

public class User
{
    public UserId Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string PasswordHash { get; set; }
    public UserImage? UserImage { get; private set; }
    public List<Role> Roles { get; set; } = new();
    public Cart? Cart { get; set; }
    public List<Order> Orders { get; set; } = new();
    public List<Product> FavoriteProducts { get; private set; } = new();
    
    private User(UserId id, string email, string? name, string passwordHash)
    {
        Id = id;
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
    }

    public static User New(UserId id, string email, string? name, string passwordHash)
        => new(id, email, name, passwordHash);

    public void UpdateUser(string email, string? name)
    {
        Email = email;
        Name = name;
    }
    
    public void UpdateUserImage(UserImage userImage)
    => UserImage = userImage;
}