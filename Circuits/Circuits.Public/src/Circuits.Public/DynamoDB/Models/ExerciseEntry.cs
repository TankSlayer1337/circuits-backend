using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.DynamoDB.Models
{
    public class ExerciseEntry
    {
        [DynamoDBHashKey(typeof(UserIdPropertyConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(ExerciseIdPropertyConverter))]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string Name { get; init; } = string.Empty;

        [DynamoDBProperty("RepetitionType", typeof(EnumPropertyConverter))]
        public RepetitionType RepetitionType { get; init; }

        [DynamoDBProperty("DefaultEquipmentPrimaryKey")]
        public string DefaultEquipmentPrimaryKey { get; init; } = string.Empty;
    }
}
