namespace Circuits.Public.Models
{
    public class PerformedExercise
    {
        public string Name { get; init; } = string.Empty;
        public List<ExerciseSet> Sets { get; init; } = new();
    }
}
