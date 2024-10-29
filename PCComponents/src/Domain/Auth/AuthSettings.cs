namespace Domain.Auth;

public class AuthSettings
{
    public static string UserRole = "User";
    public static string AdminRole = "Administrator";

    public static readonly List<string> ListOfRoles = new()
    {
        UserRole,
        AdminRole
    };
}