namespace Domain.Categories;

public record CategotyId(Guid Value)
{
    public static CategotyId New() => new(Guid.NewGuid());
    public static CategotyId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}