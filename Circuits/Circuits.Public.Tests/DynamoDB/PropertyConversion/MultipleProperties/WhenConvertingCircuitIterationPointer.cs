using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingCircuitIterationPointer : MultiplePropertiesConversionTest<CircuitIterationPointer, CircuitIterationPointerConverter>
    {
        protected override (CircuitIterationPointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var userId = TestData.CreateRandomGuidValue();
            var circuitId = TestData.CreateRandomGuidValue();
            var iterationId = TestData.CreateRandomGuidValue();
            var propertyInstance = new CircuitIterationPointer
            {
                UserId = userId,
                CircuitId = circuitId,
                IterationId = iterationId
            };
            var entryValue = $"UserId#{userId}#CircuitId#{circuitId}#IterationId#{iterationId}";
            return (propertyInstance, entryValue);
        }
    }
}
