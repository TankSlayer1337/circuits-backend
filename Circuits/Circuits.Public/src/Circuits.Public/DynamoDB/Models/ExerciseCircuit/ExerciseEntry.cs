using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class ExerciseEntry
    {
        [DynamoDBHashKey(typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(typeof(ExerciseIdConverter))]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.Name)]
        public string Name { get; init; } = string.Empty;

        [DynamoDBProperty("RepetitionType", typeof(EnumPropertyConverter))]
        public RepetitionType RepetitionType { get; init; }

        [DynamoDBProperty("DefaultEquipmentId")]
        public string? DefaultEquipmentId { get; init; } = string.Empty;
    }
}
