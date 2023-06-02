using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingCircuitItemPointer : MultiplePropertiesConversionTest<CircuitItemPointer, CircuitItemPointerConverter>
    {
        protected override (CircuitItemPointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var propertyInstance = new CircuitItemPointer
            {
                CircuitId = TestData.CreateRandomGuidValue(),
                ItemId = TestData.CreateRandomGuidValue(),
            };
            var expectedEntryValue = $"CircuitId#{propertyInstance.CircuitId}#ItemId#{propertyInstance.ItemId}";
            return (propertyInstance, expectedEntryValue);
        }
    }
}
