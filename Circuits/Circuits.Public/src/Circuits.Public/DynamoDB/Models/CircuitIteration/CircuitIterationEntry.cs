using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration
{
    public class CircuitIterationEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(CircuitPointerConverter))]
        public CircuitPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(AttributeNames.SK, typeof(IterationIdConverter))]
        public string IterationId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.DateStarted)]
        public string DateStarted { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.DateCompleted)]
        public string DateCompleted { get; init; } = string.Empty;
    }
}
