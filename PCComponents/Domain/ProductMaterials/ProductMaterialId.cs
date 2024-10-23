namespace Domain.ProductMaterials;

public record ProductMaterialId(Guid Value)
{
    public static ProductMaterialId New() => new(Guid.NewGuid());
    public static ProductMaterialId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}