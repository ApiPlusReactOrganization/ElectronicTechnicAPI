namespace Domain.FormFactors;

public record FormFactorId(Guid Value)
{
    public static FormFactorId New() => new(Guid.NewGuid());
    public static FormFactorId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}