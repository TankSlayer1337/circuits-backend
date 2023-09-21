namespace Circuits.Public.PresentationModels.CircuitDefinitionModels
{
    public class Item
    {
        public string ItemId { get; init; } = string.Empty;
        public uint Index { get; init; }
        public uint OccurrenceWeight { get; init; } = 1;
        public Exercise Exercise { get; init; }
    }
}
