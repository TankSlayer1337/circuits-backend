namespace Circuits.Public.Models
{
    public class Repetition
    {
        public List<Equipment> EquipmentItems { get; init; } = new();
        public string RepetitionType { get; init; } = string.Empty; // e.g. movement, duration
        public float RepetitionSize { get; init; }  // e.g. number of repetitions, number of seconds performed
        public int NumberOfRepetitions { get; init; } = 1;
    }
}
