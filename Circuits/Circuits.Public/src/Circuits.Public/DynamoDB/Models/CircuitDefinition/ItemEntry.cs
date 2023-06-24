using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.CircuitDefinition
{
    public class ItemEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(CircuitItemPointerConverter))]
        public CircuitItemPointer Pointer { get; init; }

        [DynamoDBRangeKey(AttributeNames.SK, typeof(ItemIdConverter))]
        public string ItemId { get; init; } = string.Empty;

        [DynamoDBProperty("Index")]
        public uint Index { get; init; }

        [DynamoDBProperty(AttributeNames.ID1, typeof(ExerciseIdConverter))]
        public string ExerciseId { get; init; } = string.Empty;

        [DynamoDBProperty("OccurrenceWeight")]
        public uint OccurrenceWeight { get; init; }

        public static ItemEntry FromRequest(string userId, AddItemRequest request)
        {
            var pointer = new CircuitItemPointer
            {
                UserId = userId,
                CircuitId = request.CircuitId
            };
            return new ItemEntry
            {
                Pointer = pointer,
                ItemId = Guid.NewGuid().ToString(),
                Index = request.Index,
                ExerciseId = request.ExerciseId,
                OccurrenceWeight = request.OccurrenceWeight
            };
        }
    }
}
