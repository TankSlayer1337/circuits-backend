using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance
{
    public class EquipmentInstanceEntry
    {
        [DynamoDBHashKey(typeof(CircuitIterationPointerConverter))]
        public CircuitIterationPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(typeof(EquipmentInstancePointerConverter))]
        public EquipmentInstancePointer EquipmentInstancePointer { get; init; }

        [DynamoDBProperty(AttributeNames.EquipmentId)]
        public string EquipmentId { get; init; }

        [DynamoDBProperty(AttributeNames.Count)]
        public int Count { get; init; }

        [DynamoDBProperty(AttributeNames.Load)]
        public float Load { get; init; }

        [DynamoDBProperty(AttributeNames.LoadUnit)]
        public float LoadUnit { get; init; }
    }
}
