using Domain.Products;

namespace Domain.ProductMaterials
{
    public class ProductMaterial
    {
        public ProductMaterialId Id { get; } // Унікальний ідентифікатор виробника
        public string Name { get; private set; } // Назва виробника
        public List<Product> Products { get; } = new();

        // Конструктор для Manufacturer
        private ProductMaterial(ProductMaterialId id, string name)
        {
            Id = id;
            Name = name;
        }

        // Метод для створення нового екземпляра Manufacturer
        public static ProductMaterial New(ProductMaterialId id, string name)
        {
            return new ProductMaterial(id, name);
        }

        // Метод для оновлення назви виробника
        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
