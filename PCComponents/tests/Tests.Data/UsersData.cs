using Api.Dtos.Authentications;
using Api.Dtos.Users;
using Domain.Authentications.Users;

namespace Tests.Data;

public static class UsersData
{
    public static UserDto MainUser => new(
        id: Guid.NewGuid(),
        email: "mainuser@example.com",
        name: "Main User",
        image : null,
        roles: new List<RoleDto> { new("User") });

    public static SignUpDto SignUpMainUser => new(
        email: "mainuser@gmail.com",
        password: "MainUserPassword123!",
        name: "Main User");
    
    public static SignUpDto SignUpDeleteUser => new(
        email: "deleteuser@gmail.com",
        password: "DeleteUserPassword123!",
        name: "Delete User");
    
    public static SignUpDto SignUpUpdateUserRoles => new(
        email: "updateuserRoles@gmail.com",
        password: "UpdateUserRolesPassword123!",
        name: "UpdateRoles User");
    public static SignUpDto SignUpNotUpdateUserRoles => new(
        email: "notupdateuserRoles@gmail.com",
        password: "notUpdateUserRolesPassword123!",
        name: "notUpdateRoles User");
    
    public static SignUpDto SignUpUploadUser => new(
        email: "uploadUseruser@gmail.com",
        password: "UploadUserUserPassword123!",
        name: "UploadUser User");
    
    public static SignUpDto invalidSignUpRequest => new(
        email: "",
        password: "123",
        name: "Invalid User"
        );
}