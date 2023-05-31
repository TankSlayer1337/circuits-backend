using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance
{
    public class EquipmentInstanceEntry
    {
        [DynamoDBHashKey(typeof(CircuitIterationPointerConverter))]
        public CircuitIterationPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(typeof(EquipmentInstancePointerConverter))]
        public EquipmentInstancePointer EquipmentInstancePointer { get; init; }

        [DynamoDBProperty(AttributeNames.ID1, typeof(EquipmentIdConverter))]
        public string EquipmentId { get; init; }

        [DynamoDBProperty(AttributeNames.Count)]
        public int Count { get; init; }

        [DynamoDBProperty(AttributeNames.Load)]
        public float Load { get; init; }

        [DynamoDBProperty(AttributeNames.LoadUnit)]
        public float LoadUnit { get; init; }
    }
}
