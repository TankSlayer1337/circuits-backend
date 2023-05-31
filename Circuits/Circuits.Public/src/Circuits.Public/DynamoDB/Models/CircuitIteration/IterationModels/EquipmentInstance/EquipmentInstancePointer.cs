using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance
{
    public class EquipmentInstancePointer : ExerciseSetPointer
    {
        public string EquipmentInstanceId { get; init; } = string.Empty;
    }
}
