using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.DynamoDBModels
{
    public class ExerciseCircuitEntry : TableEntry
    {
        [DynamoDBHashKey(typeof(UserIdPropertyConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(CircuitIdPropertyConverter))]
        public string CircuitId { get; init; } = string.Empty;

        [DynamoDBProperty("Data")]
        public string SerializedJsonData { get; init; } = string.Empty;
    }
}
