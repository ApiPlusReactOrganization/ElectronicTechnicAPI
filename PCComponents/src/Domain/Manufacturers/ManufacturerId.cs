namespace Domain.Manufacturers;

public record ManufacturerId(Guid Value)
{
    public static ManufacturerId New() => new(Guid.NewGuid());
    public static ManufacturerId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}