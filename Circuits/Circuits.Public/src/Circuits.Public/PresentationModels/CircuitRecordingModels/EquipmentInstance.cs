namespace Circuits.Public.PresentationModels.CircuitRecordingModels
{
    public class EquipmentInstance
    {
        public string EquipmentId { get; init; } = string.Empty;
        public int Count { get; init; } = 1;
        public float Load { get; init; }
        public LoadUnit LoadUnit { get; init; }
    }
}
