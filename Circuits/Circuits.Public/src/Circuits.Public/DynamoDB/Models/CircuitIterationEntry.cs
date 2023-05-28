using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.DynamoDBModels
{
    public class CircuitIterationEntry : TableEntry
    {
        [DynamoDBHashKey(typeof(UserIdPropertyConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(CircuitIterationPointerConverter))]
        public CircuitIterationPointer CircuitIterationPointer { get; init; }
    }
}
