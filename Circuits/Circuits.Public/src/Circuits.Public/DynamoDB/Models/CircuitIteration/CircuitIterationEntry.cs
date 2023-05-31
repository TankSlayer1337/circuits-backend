using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.Models.BaseModels;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration
{
    public class CircuitIterationEntry : TableEntry
    {
        [DynamoDBHashKey(typeof(CircuitPointerConverter))]
        public CircuitPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(typeof(IterationIdConverter))]
        public string IterationId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.DateStarted)]
        public string DateStarted { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.DateCompleted)]
        public string DateCompleted { get; init; } = string.Empty;
    }
}
