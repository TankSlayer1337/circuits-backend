using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class CircuitItemEntry
    {
        [DynamoDBHashKey(typeof(CircuitItemPointerConverter))]
        public CircuitItemPointer Pointer { get; init; }

        [DynamoDBRangeKey(PropertyConverterConstants.ItemId, typeof(ItemIdConverter))]
        public string ItemId { get; init; } = string.Empty;

        [DynamoDBProperty("Index")]
        public int Index { get; init; }

        [DynamoDBProperty(AttributeNames.ID1, typeof(ExerciseIdConverter))]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty("OccurrenceWeight")]
        public int OccurrenceWeight { get; init; }
    }
}
