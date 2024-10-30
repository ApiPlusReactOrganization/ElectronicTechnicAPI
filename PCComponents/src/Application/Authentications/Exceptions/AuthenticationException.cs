using Domain.Authentications.Users;

namespace Application.Authentications.Exceptions;

public abstract class AuthenticationException(UserId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public UserId UserId { get; } = id;
}

public class UserByThisEmailAlreadyExistsException(UserId id) : AuthenticationException(id, $"User by this email already exists! User id: {id}");
public class EmailOrPasswordAreIncorrect() : AuthenticationException(UserId.Empty, "Emair or Password are incorrect!");

public class AuthenticationUnknownException(UserId id, Exception innerException)
    : AuthenticationException(id, $"Unknown exception for the user under id: {id}", innerException);