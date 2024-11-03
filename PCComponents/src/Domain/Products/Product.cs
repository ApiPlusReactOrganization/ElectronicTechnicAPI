using Domain.Categories;
using Domain.Manufacturers;

namespace Domain.Products
{
    public class Product
    {
        public ProductId Id { get; }
        public ComponentCharacteristic ComponentCharacteristic { get; private init; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int StockQuantity { get; private set; }
        public ManufacturerId ManufacturerId { get; set; } 
        public Manufacturer? Manufacturer { get; set; }
        public CategoryId CategoryId { get; set; } 
        public Category? Category { get; set; }
        

        private Product(ProductId id, string name, decimal price, string description, int stockQuantity, ManufacturerId manufacturerId, CategoryId categoryId)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
        } 
        
        public static Product New(ProductId id, string name, decimal price, string description, int stockQuantity, 
            ManufacturerId manufacturerId, CategoryId categoryId , ComponentCharacteristic componentCharacteristic)
            => new(id, name, price, description, stockQuantity, manufacturerId, categoryId) {ComponentCharacteristic = componentCharacteristic};
        
        public void UpdateDetails(string name, decimal price, string description, int stockQuantity)
        {
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
        
    }
}