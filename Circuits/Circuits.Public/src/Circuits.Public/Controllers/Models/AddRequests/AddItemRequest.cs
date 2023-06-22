namespace Circuits.Public.Controllers.Models.AddRequests
{
    public class AddItemRequest
    {
        public string CircuitId { get; init; } = string.Empty;
        public uint Index { get; init; }
        public string ExerciseId { get; init; } = string.Empty;
        public uint OccurrenceWeight { get; init; } = 1;
    }
}
