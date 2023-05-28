namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class ExerciseCircuit
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public List<CircuitItem> CircuitItems { get; init; } = new List<CircuitItem>();
    }
}
