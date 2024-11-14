using Domain.Authentications.Roles;

namespace Domain.Authentications.Users;

public class User
{
    public UserId Id { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
    public string PasswordHash { get; set; }
    public UserImage? UserImage { get; private set; }

    public List<Role> Roles { get; set; } = new();

    private User(UserId id, string email, string? name, string passwordHash)
    {
        Id = id;
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
    }

    public static User New(UserId id, string email, string? name, string passwordHash)
        => new(id, email, name, passwordHash);
    
    public void UpdateUserImage(UserImage userImage)
    => UserImage = userImage;
}