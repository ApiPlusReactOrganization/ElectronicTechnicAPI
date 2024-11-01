using Application.Authentications.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class AuthenticationErrorHandler
{
    public static ObjectResult ToObjectResult(this AuthenticationException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                UserByThisEmailAlreadyExistsException => StatusCodes.Status409Conflict,
                EmailOrPasswordAreIncorrect => StatusCodes.Status401Unauthorized,
                AuthenticationUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Authentication error handler does not implemented")
            }
        };
    }
}