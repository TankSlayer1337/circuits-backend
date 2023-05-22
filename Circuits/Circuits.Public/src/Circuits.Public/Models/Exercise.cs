namespace Circuits.Public.Models
{
    public class Exercise
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public float Weight { get; init; } = 1; // the weight in the exercise list: how often this exercise shows up.
        public string RepetitionType { get; init; } = string.Empty; // e,g, movement, duration
        public float RepetitionUnit { get; init; }  // e.g. seconds, iterations
    }
}
