using Domain.Manufacturers;

namespace Application.Manufacturers.Exceptions;


public abstract class ManufacturerException(ManufacturerId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ManufacturerId ManufacturerId { get; } = id;
}
public class ManufacturerAlreadyExistsException(ManufacturerId id) : ManufacturerException(id, $"Manufacturer already exists: {id}");

public class ManufacturerUnknownException(ManufacturerId id, Exception innerException)
    : ManufacturerException(id, $"Unknown exception for the Manufacturer under id: {id}", innerException);