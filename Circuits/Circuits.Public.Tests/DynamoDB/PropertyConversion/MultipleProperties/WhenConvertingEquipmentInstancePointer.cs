using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingEquipmentInstancePointer : MultiplePropertiesConversionTest<EquipmentInstancePointer, EquipmentInstancePointerConverter>
    {
        protected override (EquipmentInstancePointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var itemId = TestData.CreateRandomGuidValue();
            var occurrenceId = TestData.CreateRandomGuidValue();
            var setId = TestData.CreateRandomGuidValue();
            var equipmentInstanceId = TestData.CreateRandomGuidValue();
            var propertyInstance = new EquipmentInstancePointer
            {
                ItemId = itemId,
                OccurrenceId = occurrenceId,
                SetId = setId,
                EquipmentInstanceId = equipmentInstanceId
            };
            var entryValue = $"ItemId#{itemId}#OccurrenceId#{occurrenceId}#SetId#{setId}#EquipmentInstanceId#{equipmentInstanceId}";
            return (propertyInstance,  entryValue);
        }
    }
}
