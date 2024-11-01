using Domain.Authentications.Roles;

namespace Domain.Authentications.Users;

public class User
{
    public UserId Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string PasswordHash { get; set; }
    public string? Image { get; set; }
    public List<Role> Roles { get; set; } = new();

    public User(UserId id, string email, string? name, string passwordHash, string? image)
    {
        Id = id;
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        Image = image;
    }

    public static User New(UserId id, string email, string? name, string passwordHash, string? image = null)
        => new(id, email, name, passwordHash, image);
}