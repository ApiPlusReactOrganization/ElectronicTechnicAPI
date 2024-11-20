using Api.Dtos.Authentications;

namespace Tests.Data;

public static class AccountData
{
    public static SignUpDto SignUpRequest => new(
        email: "testuser@example.com",
        password: "StrongPassword123!",
        name: "Test User");

    public static SignInDto SignInRequest => new(
        email: "loginedtestuser@example.com",
        password: "StrongPassword123!");

    public static SignUpDto SignUpWithOutPasswordRequest => new(
        email: "testuser@example.com",
        password: null,
        name: "Test User");
    
    public static SignUpDto SignUpWithInvalidEmailRequest => new(
        email: "invalid-email",
        password: "Password123!",
        name: "Test User");

    public static SignUpDto SignUpWithoutNameRequest => new(
        email: "testuser@example.com",
        password: "Password123!",
        name: string.Empty);
    
    public static SignUpDto SignUpForSignInRequest => new(
        email: "loginedtestuser@example.com",
        password: "StrongPassword123!",
        name: "Test User");
    public static SignInDto SignInWithInvalidCredentialsRequest => new(
        email: "loginedtestuser@example.com",
        password: "WrongPassword123!");

    public static SignInDto SignInWithInvalidEmailRequest => new(
        email: "invalid-email",
        password: "Password123!");
    
    public static SignInDto SignInWithoutPasswordRequest => new(
        email: "loginedtestuser@example.com",
        password: null);
}