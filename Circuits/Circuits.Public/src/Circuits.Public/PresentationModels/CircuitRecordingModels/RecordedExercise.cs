namespace Circuits.Public.PresentationModels.CircuitRecordingModels
{
    public class RecordedExercise
    {
        public string ExerciseId { get; init; } = string.Empty;
        public string DateCompleted { get; init; } = string.Empty;
        public ExerciseSet ExerciseSet { get; init; }
    }
}
