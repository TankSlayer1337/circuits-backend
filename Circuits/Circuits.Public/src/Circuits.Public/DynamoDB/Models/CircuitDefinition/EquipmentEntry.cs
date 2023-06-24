using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.CircuitDefinition
{
    public class EquipmentEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(AttributeNames.SK, typeof(EquipmentIdConverter))]
        public string EquipmentId { get; init; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string Name { get; init; } = string.Empty;

        [DynamoDBProperty("CanBeUsedInMultiples")]
        public bool CanBeUsedInMultiples { get; init; }

        public static EquipmentEntry FromRequest(string userId, AddEquipmentRequest request)
        {
            return new EquipmentEntry
            {
                UserId = userId,
                EquipmentId = Guid.NewGuid().ToString(),
                Name = request.Name,
                CanBeUsedInMultiples = request.CanBeUsedInMultiples
            };
        }
    }
}
