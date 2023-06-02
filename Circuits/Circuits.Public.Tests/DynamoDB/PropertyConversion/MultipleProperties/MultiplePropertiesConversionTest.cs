using Amazon.DynamoDBv2.DataModel;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public abstract class MultiplePropertiesConversionTest<TProperty, TPropertyConverter>
        where TProperty : class, new()
        where TPropertyConverter : class, IPropertyConverter, new()
    {
        private readonly TPropertyConverter _converter = new();

        [Fact]
        public void FromEntry()
        {
            // GIVEN an entry value and corresponding expected result
            var (expectedResult, entryValue) = CreatePropertyAndStringRepresentation();

            // WHEN converting from entry
            var result = _converter.FromEntry(entryValue);

            // THEN the entry value should be converted into the expected result
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ToEntry()
        {
            // GIVEN a property value and corresponding expected entry value
            var (propertyValue, expectedEntryValue) = CreatePropertyAndStringRepresentation();

            // WHEN converting from entry
            var result = _converter.ToEntry(propertyValue);

            // THEN the result should be the expected entry value
            result.AsString().Should().Be(expectedEntryValue);
        }

        protected abstract (TProperty propertyInstance, string entryValue) CreatePropertyAndStringRepresentation();
    }
}
