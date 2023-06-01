using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingCircuitItemPointer
    {
        private readonly Faker _faker = new();
        private readonly CircuitItemPointerConverter _converter = new();

        [Fact]
        public void FromEntry()
        {
            // GIVEN an entry value
            var circuitId = CreateRandomGuidValue();
            var itemId = CreateRandomGuidValue();
            var entryValue = $"CircuitId#{circuitId}#ItemId#{itemId}";

            // GIVEN a corresponding expected result
            var expectedResult = new CircuitItemPointer
            {
                CircuitId = circuitId,
                ItemId = itemId
            };

            // WHEN converting from entry
            var result = _converter.FromEntry(entryValue);

            // THEN the entry value should be converted into the expected result
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToEntry()
        {
            // GIVEN a CircuitItemPointer
            var pointer = new CircuitItemPointer
            {
                CircuitId = CreateRandomGuidValue(),
                ItemId = CreateRandomGuidValue()
            };

            // GIVEN a corresponding expected entry value
            var expectedEntryValue = $"CircuitId#{pointer.CircuitId}#ItemId#{pointer.ItemId}";

            // WHEN converting to entry
            var result = _converter.ToEntry(pointer);

            // THEN the result should be the expected entry value
            result.AsString().Should().Be(expectedEntryValue);
        }

        private string CreateRandomGuidValue()
        {
            return _faker.Random.Guid().ToString();
        }
    }
}
