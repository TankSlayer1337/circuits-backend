using Circuits.Public.PresentationModels.CircuitRecordingModels;

namespace Circuits.Public.Controllers.Models.IterationModels
{
    public struct AddEquipmentInstanceRequest
    {
        public string CircuitId { get; init; }
        public string IterationId { get; init; }
        public string ItemId { get; init; }
        public string OccurrenceId { get; init; }
        public string SetId { get; init; }
        public string EquipmentId { get; init; }
        public int Count { get; init; }
        public float Load { get; init; }
        public LoadUnit LoadUnit { get; init; }
    }
}
