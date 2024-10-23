using Domain.ProductMaterials;

namespace Domain.Products
{
    public abstract class Product
    {
        public ProductId Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int StockQuantity { get; private set; }
        public List<ProductMaterial> ProductMaterials { get; } = new();

        protected Product(ProductId id, string name, decimal price, string description, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }

        public void UpdateDetails(string name, decimal price, string description, int stockQuantity)
        {
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
        
    }
}