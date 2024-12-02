using Domain.Categories;
using Domain.Manufacturers;

namespace Application.Manufacturers.Exceptions;


public abstract class ManufacturerException(ManufacturerId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public ManufacturerId ManufacturerId { get; } = id;
}

public class ManufacturerNotFoundException(ManufacturerId id) 
    : ManufacturerException(id, $"Manufacturer under id: {id} not found");

public class ManufacturerAlreadyExistsException(ManufacturerId id) 
    : ManufacturerException(id, $"Manufacturer already exists: {id}");

public class CategoryNotFoundException(CategoryId id) 
    : ManufacturerException(ManufacturerId.Empty, $"Category under id: {id} not found");

public class ManufacturerUnknownException(ManufacturerId id, Exception innerException)
    : ManufacturerException(id, $"Unknown exception for the Manufacturer under id: {id}", innerException);
    
public class ManufacturerHasRelatedProductsException(ManufacturerId id)
    : ManufacturerException(id, $"Manufacturer with ID {id} cannot be deleted because it has related products.");