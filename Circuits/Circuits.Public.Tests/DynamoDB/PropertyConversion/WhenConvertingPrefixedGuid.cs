namespace Circuits.Public.Tests.DynamoDB.PropertyConversion
{
    public class WhenConvertingPrefixedGuid
    {
        [Fact]
        public void FromEntry()
        {
            // GIVEN a PrefixedGuidConverter instance
            var testConverter = new PrefixedGuidTestConverter();

            // GIVEN a prefixed GUID value
            var guidValue = Guid.NewGuid().ToString();
            var entryValue = $"{testConverter.Prefix}#{guidValue}";

            // WHEN converting from entry
            var result = (string)testConverter.FromEntry(entryValue);

            // THEN the result should be equal to the guid value without prefix
            result.Should().Be(guidValue);
        }

        [Fact]
        public void ToEntry()
        {
            // GIVEN a PrefixedGuidConverter instance
            var testConverter = new PrefixedGuidTestConverter();

            // GIVEN a GUID value
            var guidValue = Guid.NewGuid().ToString();

            // WHEN converting to entry
            var result = testConverter.ToEntry(guidValue);

            // THEN the entry value should have the correct prefixed value
            result.AsString().Should().Be($"{testConverter.Prefix}#{guidValue}");
        }
    }
}
