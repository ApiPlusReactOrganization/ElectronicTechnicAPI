using System.Reflection.Metadata;
using Domain.Authentications.Users;

namespace Application.Users.Exceptions;

public abstract class UserException(UserId id, string message, Exception innerException = null)
: Exception(message, innerException)
{
    public UserId UserId { get; } = id;
}

public class UserByThisEmailAlreadyExistsException(UserId id) : UserException(id, $"User by this email already exists! User id: {id}");
public class EmailOrPasswordAreIncorrect() : UserException(UserId.Empty, "Email or Password are incorrect!");
public class UserNotFoundException(UserId id) : UserException(id, $"User under id: {id} was not found!");
public class RoleNotFoundException(string role) : UserException(UserId.Empty, $"Role under name: {role} was not found!");

public class UserUnknownException(UserId id, Exception innerException)
    : UserException(id, $"Unknown exception for the user under id: {id}", innerException);