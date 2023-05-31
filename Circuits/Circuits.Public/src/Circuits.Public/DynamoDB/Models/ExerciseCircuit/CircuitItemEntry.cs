using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class CircuitItemEntry
    {
        [DynamoDBHashKey(typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(CircuitItemPointerConverter))]
        public CircuitItemPointer Pointer { get; init; }

        [DynamoDBProperty("Index")]
        public int Index { get; init; }

        [DynamoDBProperty(AttributeNames.ExerciseId)]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty("OccurrenceWeight")]
        public int OccurrenceWeight { get; init; }
    }
}
