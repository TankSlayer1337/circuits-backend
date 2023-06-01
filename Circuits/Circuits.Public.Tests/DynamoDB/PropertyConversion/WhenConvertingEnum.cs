using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion
{
    public class WhenConvertingEnum
    {
        private readonly Faker _faker = new();

        [Fact]
        public void FromEntry()
        {
            // GIVEN an enum value
            var enumValue = _faker.Random.Enum<TestEnum>();

            // GIVEN enum value is represented as int in entry
            var entryValue = (int)enumValue;

            // GIVEN an enum converter instance
            var enumConverter = new EnumPropertyConverter<TestEnum>();

            // WHEN converting from entry
            var result = enumConverter.FromEntry(entryValue);

            // THEN the result should be the correct enum value
            result.Should().Be(enumValue);
        }

        [Fact]
        public void ToEntry()
        {
            // GIVEN an enum value
            var enumValue = _faker.Random.Enum<TestEnum>();

            // GIVEN an enum converter instance
            var enumConverter = new EnumPropertyConverter<TestEnum>();

            // WHEN converting to entry
            var result = enumConverter.ToEntry(enumValue);

            // THEN the result should be the correct int value
            result.AsInt().Should().Be((int)enumValue);
        }
    }
}
