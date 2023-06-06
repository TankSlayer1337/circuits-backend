using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class CircuitEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(AttributeNames.SK, typeof(CircuitIdConverter))]
        public string CircuitId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.Name)]
        public string Name { get; init; } = string.Empty;

        public static CircuitEntry Create(string userId, string name)
        {
            return new CircuitEntry
            {
                UserId = userId,
                CircuitId = Guid.NewGuid().ToString(),
                Name = name
            };
        }
    }
}
