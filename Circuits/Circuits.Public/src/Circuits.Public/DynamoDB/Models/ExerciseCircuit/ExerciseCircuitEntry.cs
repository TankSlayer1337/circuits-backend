using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.Models.BaseModels;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class ExerciseCircuitEntry : TableEntry
    {
        [DynamoDBHashKey(typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(CircuitIdConverter))]
        public string CircuitId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.Name)]
        public string Name { get; init; } = string.Empty;
    }
}
