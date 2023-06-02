using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingCircuitPointer : MultiplePropertiesConversionTest<CircuitPointer, CircuitPointerConverter>
    {
        protected override (CircuitPointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var propertyInstance = new CircuitPointer
            {
                UserId = TestData.CreateRandomGuidValue(),
                CircuitId = TestData.CreateRandomGuidValue()
            };
            var entryValue = $"UserId#{propertyInstance.UserId}#CircuitId#{propertyInstance.CircuitId}";
            return (propertyInstance,  entryValue);
        }
    }
}
