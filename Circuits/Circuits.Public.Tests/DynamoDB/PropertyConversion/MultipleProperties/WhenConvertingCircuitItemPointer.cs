using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingCircuitItemPointer : MultiplePropertiesConversionTest<CircuitItemPointer, CircuitItemPointerConverter>
    {
        protected override (CircuitItemPointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var propertyInstance = new CircuitItemPointer
            {
                UserId = TestData.CreateRandomGuidValue(),
                CircuitId = TestData.CreateRandomGuidValue(),
            };
            var expectedEntryValue = $"UserId#{propertyInstance.UserId}#CircuitId#{propertyInstance.CircuitId}";
            return (propertyInstance, expectedEntryValue);
        }
    }
}
