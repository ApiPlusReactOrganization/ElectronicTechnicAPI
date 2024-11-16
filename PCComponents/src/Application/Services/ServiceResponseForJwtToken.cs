namespace Application.Services;

public class ServiceResponseForJwtToken
{
    public string Message { get; set; }
    public object? Payload { get; set; }

    public static ServiceResponseForJwtToken GetResponse(string message, object? payload)
    {
        return new ServiceResponseForJwtToken
        {
            Message = message,
            Payload = payload,
        };
    }
}