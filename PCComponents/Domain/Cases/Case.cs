using Domain.FormFactors;
using Domain.Manufacturers;
using Domain.Products;

namespace Domain.Cases
{
    public class Case : Product
    {
        // Поля специфічні для корпусу
        public int NumberOfFans { get; set; } // Максимальна довжина відеокарти
        public string CoolingSystem { get; set; } // Система охолодження

        // Зв'язок із таблицею Manufacturer
        public ManufacturerId ManufacturerId { get; set; } // Ідентифікатор виробника
        public Manufacturer Manufacturer { get; set; } // Навігаційна властивість до виробника

        // Зв'язок із таблицею FormFactor
        public List<FormFactor> FormFactors { get; } = new();

        // Конструктор для Case
        public Case(ProductId id, string name, decimal price, string description, int stockQuantity,
            int numberOfFans, string coolingSystem, ManufacturerId manufacturerId)
            : base(id, name, price, description, stockQuantity)
        {
            NumberOfFans = numberOfFans;
            CoolingSystem = coolingSystem;
            ManufacturerId = manufacturerId;
        }

        // Метод для створення нового екземпляра Case
        public static Case New(ProductId id, string name, decimal price, string description, int stockQuantity,
            int maxGpuLength, string coolingSystem, ManufacturerId manufacturerId)
        {
            return new Case(id, name, price, description, stockQuantity, maxGpuLength, coolingSystem,
                manufacturerId);
        }

        // Метод для оновлення деталей корпусу
        public void UpdateCaseDetails(string name, decimal price, string description, int stockQuantity,
            int maxGpuLength, string coolingSystem)
        {
            // Оновлюємо базові поля
            UpdateDetails(name, price, description, stockQuantity);

            // Оновлюємо специфічні поля
            NumberOfFans = maxGpuLength;
            CoolingSystem = coolingSystem;
        }
    }
}