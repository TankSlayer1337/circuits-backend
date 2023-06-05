namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class CircuitItem
    {
        public string ItemId { get; init; } = string.Empty;
        public int Index { get; init; }
        public int OccurrenceWeight { get; init; } = 0;
        public Exercise Exercise { get; init; }
    }
}
