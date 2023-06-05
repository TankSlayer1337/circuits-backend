using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
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
    }
}
