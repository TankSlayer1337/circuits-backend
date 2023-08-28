namespace Circuits.Public.Controllers.Models.IterationModels
{
    public readonly struct AddRecordedExerciseRequest
    {
        public string CircuitId { get; init; }
        public string IterationId { get; init; }
        public string ItemId { get; init; }
        public string ExerciseId { get; init; }
        
        public AddRecordedExerciseRequest(string circuitId, string iterationId, string itemId, string exerciseId)
        {
            CircuitId = circuitId;
            IterationId = iterationId;
            ItemId = itemId;
            ExerciseId = exerciseId;
        }
    }
}
