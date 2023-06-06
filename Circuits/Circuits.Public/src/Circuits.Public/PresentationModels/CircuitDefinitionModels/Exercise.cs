namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class Exercise
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public RepetitionType RepetitionType { get; init; }
        public Equipment? DefaultEquipment { get; init; }
    }
}
