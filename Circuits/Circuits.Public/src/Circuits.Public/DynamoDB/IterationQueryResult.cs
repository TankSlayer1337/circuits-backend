using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;

namespace Circuits.Public.DynamoDB
{
    public class IterationQueryResult
    {
        public List<RecordedExerciseEntry> ExerciseEntries { get; init; } = new List<RecordedExerciseEntry>();
        public List<ExerciseSetEntry> ExerciseSets { get; init; } = new List<ExerciseSetEntry>();
        public List<EquipmentInstanceEntry> EquipmentInstances { get; init; } = new List<EquipmentInstanceEntry>();
    }
}
