using Domain.FormFactors;

namespace Domain.ComponentCharacteristics
{
    public class ComponentCharacteristic
    { 
        public string Type { get; }

        public Case? Case { get; private init; }
        /*
        public TechnicalEquipment? TechnicalEquipment { get; private init; }
        */

        private ComponentCharacteristic(string type)
        {
            Type = type;
        }

        public static ComponentCharacteristic NewMedicalEquipment(Case someCase)
            => new("Computer case") { Case = someCase };

        /*public static ComponentCharacteristic NewTechnicalEquipment(TechnicalEquipment technicalEquipment)
            => new("technical") { TechnicalEquipment = technicalEquipment };*/
    }

    public record Case
    {
        public required List<FormFactor> FormFactors { get; init; }
        public required int NumberOfFans { get; init; } 
        public required string CoolingSystem { get; init; }
    }

    /*public record TechnicalEquipment
    {
        public required string Name { get; init; }
        public required double Weight { get; init; }

        public required List<TechnicalEquipmentPart> Parts { get; init; }
    }

    public record TechnicalEquipmentPart
    {
        public required string Name { get; init; }
    }*/
}
