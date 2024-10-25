namespace Domain.ComponentCharacteristics;

public record ComponentCharacteristicId(Guid Value)
{
    public static ComponentCharacteristicId New() => new(Guid.NewGuid());
    public static ComponentCharacteristicId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}