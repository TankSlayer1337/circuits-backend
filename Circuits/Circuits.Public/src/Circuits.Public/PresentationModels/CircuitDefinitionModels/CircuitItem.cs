namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class CircuitItem
    {
        public string ItemId { get; init; } = string.Empty;
        public uint Index { get; init; }
        public uint OccurrenceWeight { get; init; } = 0;
        public Exercise Exercise { get; init; }
    }
}
