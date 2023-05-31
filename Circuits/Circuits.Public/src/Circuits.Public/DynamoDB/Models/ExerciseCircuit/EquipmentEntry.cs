using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.Models.BaseModels;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class EquipmentEntry : TableEntry
    {
        [DynamoDBHashKey(typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(EquipmentIdConverter))]
        public string EquipmentId { get; init; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string EquipmentName { get; init; } = string.Empty;

        [DynamoDBProperty("CanBeUsedInMultiples")]
        public bool CanBeUsedInMultiples { get; init; }
    }
}
