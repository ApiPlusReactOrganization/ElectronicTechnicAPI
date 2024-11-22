namespace Domain.CartItems;

public record CartItemId(Guid Value)
{
    public static CartItemId New() => new(Guid.NewGuid());
    public static CartItemId Empty => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}