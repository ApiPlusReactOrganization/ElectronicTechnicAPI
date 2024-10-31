using Domain.Products;

namespace Domain.FormFactors
{
    public class FormFactor
    {
        public FormFactorId Id { get; }
        public string Name { get; private set; }

        private FormFactor(FormFactorId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static FormFactor New(FormFactorId id, string name)
        {
            return new FormFactor(id, name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}