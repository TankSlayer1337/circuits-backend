using Circuits.Public.DynamoDB.PropertyConverters;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion
{
    public class PrefixedGuidTestConverter : PrefixedGuidConverter
    {
        protected override string _prefix => Prefix;
        public string Prefix;

        public PrefixedGuidTestConverter()
        {
            var faker = new Faker();
            Prefix = faker.Random.AlphaNumeric(10);
        }
    }
}
