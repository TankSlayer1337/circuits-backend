using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.DynamoDB.Models.ExerciseCircuit
{
    public class ExerciseEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(UserIdConverter))]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey(AttributeNames.SK, typeof(ExerciseIdConverter))]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty(AttributeNames.Name)]
        public string Name { get; init; } = string.Empty;

        [DynamoDBProperty("RepetitionType", typeof(EnumPropertyConverter<RepetitionType>))]
        public RepetitionType RepetitionType { get; init; }

        [DynamoDBProperty(AttributeNames.ID1, typeof(EquipmentIdConverter))]
        public string? DefaultEquipmentId { get; init; } = string.Empty;

        public static ExerciseEntry FromRequest(AddExerciseRequest request)
        {
            return new ExerciseEntry
            {
                UserId = request.UserId,
                ExerciseId = Guid.NewGuid().ToString(),
                Name = request.Name,
                RepetitionType = request.RepetitionType,
                DefaultEquipmentId = request.DefaultEquipmentId
            };
        }
    }
}
