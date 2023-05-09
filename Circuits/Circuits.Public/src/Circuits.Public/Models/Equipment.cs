namespace Circuits.Public.Models
{
    public class Equipment
    {
        public string Name { get; init; } = string.Empty;   // e.g. Dumbbell
        public float Load { get; init; } = 0;   // e.g. 50 (kg)
        public string LoadUnit { get; init; } = string.Empty;   // e.g. kg, lbs
        public int Count { get; init; } = 0;    // e.g. 2 dumbbells => Count = 2.
    }
}
