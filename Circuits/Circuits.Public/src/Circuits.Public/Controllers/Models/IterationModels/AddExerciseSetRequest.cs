using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.Controllers.Models.IterationModels
{
    public readonly struct AddExerciseSetRequest
    {
        public string CircuitId { get; init; }
        public string IterationId { get; init; }
        public string ItemId { get; init; }
        public string OccurrenceId { get; init; }
        public string RepetitionMeasurement { get; init; }
        public int SetIndex { get; init; }
        public RepetitionType RepetitionType { get; init; }
    }
}
