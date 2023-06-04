using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Circuits.Public.DynamoDB
{
    // TODO: create interface and use it instead of this class directly.
    public class DynamoDbContextWrapper
    {
        private readonly DynamoDBContext _dynamoDbContext;

        public DynamoDbContextWrapper(AmazonDynamoDBClient dynamoDBClient)
        {
            _dynamoDbContext = new(dynamoDBClient);
        }

        public Task SaveAsync<T>(T item) where T : class
        {
            return _dynamoDbContext.SaveAsync(item);
        }
    }
}
