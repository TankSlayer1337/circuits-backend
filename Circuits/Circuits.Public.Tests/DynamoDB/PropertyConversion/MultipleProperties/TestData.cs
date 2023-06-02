namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class TestData
    {
        public static string CreateRandomGuidValue()
        {
            var faker = new Faker();
            return faker.Random.Guid().ToString();
        }
    }
}
