namespace Domain.ComponentCharacteristics
{
    public class ComponentCharacteristic
    { 
        public string Type { get; }

        public Case? Case { get; private init; }
        public Processor? Processor { get; private init; }

        private ComponentCharacteristic(string type)
        {
            Type = type;
        }

        public static ComponentCharacteristic NewCaseCharacteristic(Case someCase)
            => new("case") { Case = someCase };
        
        public static ComponentCharacteristic NewProcessorCharacteristic(Processor someProcessor)
            => new("processor") { Processor = someProcessor };
    }

    public interface ICharacteristic
    {
        
    }

    public record Case : ICharacteristic
    {
        public required string FormFactors { get; init; }
        public required int NumberOfFans { get; init; } 
        public required string CoolingSystem { get; init; }
    }
    public record Processor : ICharacteristic
    {
        public required int NumberOfStreams { get; init; }
        public required int NumberOfСores { get; init; } 
        public required string Series { get; init; } 
    }
}
