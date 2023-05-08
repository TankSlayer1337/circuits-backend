namespace Circuits.Public.Models
{
    public class Resistance
    {
        public string Type { get; init; } = string.Empty;   // e.g. DB, BB, , Rubber band, 
        public int Count { get; init; } = 1;    // e.g. 2 DBs, 1 BB, 
        public string Unit { get; init; } = string.Empty;   // e.g. kg, lbs
        public float Load { get; init; } = 0;   //  e.g. much weight
    }
}
