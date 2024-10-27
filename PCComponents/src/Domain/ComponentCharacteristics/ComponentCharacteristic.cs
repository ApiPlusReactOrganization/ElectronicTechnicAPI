namespace Domain.ComponentCharacteristics
{
    public class ComponentCharacteristic
    { 
        public string Type { get; }

        public Case? Case { get; private init; }

        private ComponentCharacteristic(string type)
        {
            Type = type;
        }

        public static ComponentCharacteristic NewMedicalEquipment(Case someCase)
            => new("Computer case") { Case = someCase };
    }

    public record Case
    {
        public required string FormFactors { get; init; }
        public required int NumberOfFans { get; init; } 
        public required string CoolingSystem { get; init; }
    }
}
