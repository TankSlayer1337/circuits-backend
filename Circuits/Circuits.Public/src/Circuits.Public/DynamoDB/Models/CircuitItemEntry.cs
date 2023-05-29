using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models
{
    public class CircuitItemEntry
    {
        [DynamoDBHashKey(typeof(UserIdPropertyConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey]
        public CircuitItemPointer Pointer { get; init; }
    }
}
