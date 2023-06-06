namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class Equipment
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;   // e.g. Dumbbell
        public bool CanBeUsedInMultiples { get; init; }
    }
}
