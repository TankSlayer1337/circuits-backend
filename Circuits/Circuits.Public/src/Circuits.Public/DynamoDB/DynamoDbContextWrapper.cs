using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Circuits.Public.DynamoDB
{
    // TODO: create interface and use it instead of this class directly.
    public class DynamoDbContextWrapper : IDynamoDbContextWrapper
    {
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly string _tableName;

        public DynamoDbContextWrapper(AmazonDynamoDBClient dynamoDBClient)
        {
            _dynamoDbContext = new(dynamoDBClient);
            _tableName = Environment.GetEnvironmentVariable("TABLE_NAME") ?? throw new NullReferenceException();
        }

        public Task SaveAsync<T>(T item) where T : class
        {
            var config = new DynamoDBOperationConfig
            {
                OverrideTableName = _tableName
            };
            return _dynamoDbContext.SaveAsync(item, config);
        }
    }
}
