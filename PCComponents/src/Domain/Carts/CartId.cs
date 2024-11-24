namespace Domain.Carts;

public record CartId(Guid Value)
{
    public static CartId New() => new(Guid.NewGuid());
    public static CartId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}