namespace Circuits.Public.Models
{
    public class ExerciseCircuit
    {
        public string Name { get; init; } = string.Empty;
        public List<Exercise> Exercises { get; init; } = new();
    }
}
