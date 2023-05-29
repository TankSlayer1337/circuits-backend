using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels
{
    public class RecordedExerciseEntry
    {
        [DynamoDBHashKey(typeof(CircuitIterationPointerConverter))]
        public CircuitIterationPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(typeof(ItemIdPropertyConverter))]
        public string ItemId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.ExerciseId)]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.DateCompleted)]
        public string DateCompleted { get; init; } = string.Empty;
    }
}
