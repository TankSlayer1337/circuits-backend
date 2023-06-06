using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Circuits.Public.DynamoDB
{
    public class DynamoDbContextWrapper : IDynamoDbContextWrapper
    {
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly DynamoDBOperationConfig _operationConfig;

        public DynamoDbContextWrapper(AmazonDynamoDBClient dynamoDBClient)
        {
            _dynamoDbContext = new(dynamoDBClient);
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME") ?? throw new NullReferenceException();
            _operationConfig = new DynamoDBOperationConfig
            {
                OverrideTableName = tableName
            };
        }

        public Task SaveAsync<T>(T item) where T : class
        {
            return _dynamoDbContext.SaveAsync(item, _operationConfig);
        }

        public Task<List<T>> QueryAsync<T>(QueryOperationConfig config) where T : class
        {
            return _dynamoDbContext.FromQueryAsync<T>(config, _operationConfig).GetRemainingAsync();
        }

        public Task<List<T>> QueryAsync<T>(object hashKeyValue, QueryOperator queryOperator, IEnumerable<object> values) where T : class
        {
            return _dynamoDbContext.QueryAsync<T>(hashKeyValue, queryOperator, values, _operationConfig).GetRemainingAsync();
        }
    }
}
