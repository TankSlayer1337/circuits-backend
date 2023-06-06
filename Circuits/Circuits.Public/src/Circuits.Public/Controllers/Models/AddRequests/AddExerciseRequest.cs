using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.Controllers.Models.AddRequests
{
    public class AddExerciseRequest
    {
        // UserId property will be removed when access token is used.
        public string UserId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public RepetitionType RepetitionType { get; init; }
        public string? DefaultEquipmentId { get; init; }
    }
}