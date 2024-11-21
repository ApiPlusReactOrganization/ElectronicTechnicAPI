namespace Domain.Products;

public class ProductImage
{
    public ProductImageId Id { get; }
    public Product Product { get; }
    public ProductId ProductId { get; }
    public string FilePath { get; private set; }

    private ProductImage(ProductImageId id, ProductId productId, string filePath)
    {
        Id = id; 
        ProductId = productId;
        FilePath = filePath;
    }

    public static ProductImage New(ProductImageId id, ProductId productId, string filePath) 
        => new ProductImage(id, productId, filePath);
}