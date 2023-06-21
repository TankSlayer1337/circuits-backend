using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.Controllers.Models.AddRequests
{
    public class AddExerciseRequest
    {
        public string Name { get; init; } = string.Empty;
        public RepetitionType RepetitionType { get; init; }
        public string? DefaultEquipmentId { get; init; }
    }
}