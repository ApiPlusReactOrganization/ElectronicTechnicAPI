using Api.Dtos.Authentications;
using Api.Dtos.Users;
using Domain.Authentications.Users;

namespace Tests.Data;

public static class UsersData
{
    /*public static UserDto MainUser => new(
        Id: Guid.NewGuid(),
        Email: "mainuser@example.com",
        Name: "Main User",
        Image : null,
        Roles: new List<RoleDto> { new("User") });*/

    public static SignUpDto SignUpMainUser => new(
        Email: "mainuser@gmail.com",
        Password: "MainUserPassword123!",
        Name: "Main User");
    
    public static SignUpDto SignUpDeleteUser => new(
        Email: "deleteuser@gmail.com",
        Password: "DeleteUserPassword123!",
        Name: "Delete User");
    
    public static SignUpDto SignUpUpdateUserRoles => new(
        Email: "updateuserRoles@gmail.com",
        Password: "UpdateUserRolesPassword123!",
        Name: "UpdateRoles User");
    public static SignUpDto SignUpNotUpdateUserRoles => new(
        Email: "notupdateuserRoles@gmail.com",
        Password: "notUpdateUserRolesPassword123!",
        Name: "notUpdateRoles User");
    
    public static SignUpDto SignUpUploadUser => new(
        Email: "uploadUseruser@gmail.com",
        Password: "UploadUserUserPassword123!",
        Name: "UploadUser User");
    
    public static SignUpDto invalidSignUpRequest => new(
        Email: "",
        Password: "123",
        Name: "Invalid User"
        );
}