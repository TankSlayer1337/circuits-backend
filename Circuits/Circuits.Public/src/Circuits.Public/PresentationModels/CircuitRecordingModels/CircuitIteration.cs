namespace Circuits.Public.PresentationModels.CircuitRecordingModels
{
    public class CircuitIteration
    {
        public string CircuitId { get; init; } = string.Empty;
        public List<RecordedExercise> RecorderExercises { get; init; } = new List<RecordedExercise>();
        public string DateStarted { get; init; } = string.Empty;
        public string DateCompleted { get; init; } = string.Empty;
    }
}
