using Domain.Cases;

namespace Domain.FormFactors
{
    public class FormFactor
    {
        public FormFactorId Id { get; } // Унікальний ідентифікатор виробника
        public string Name { get; private set; } // Назва виробника
        public List<Case> Cases { get; } = new();
        
        // Конструктор для Manufacturer
        private FormFactor(FormFactorId id, string name)
        {
            Id = id;
            Name = name;
        }

        // Метод для створення нового екземпляра Manufacturer
        public static FormFactor New(FormFactorId id, string name)
        {
            return new FormFactor(id, name);
        }

        // Метод для оновлення назви виробника
        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
