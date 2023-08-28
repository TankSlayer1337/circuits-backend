using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Circuits.Public.Utilities;

namespace Circuits.Public.DynamoDB
{
    public class DynamoDbClientWrapper : IDynamoDbClientWrapper
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly string _tableName;

        public DynamoDbClientWrapper(AmazonDynamoDBClient client, IEnvironmentVariableGetter environmentVariableGetter)
        {
            _client = client;
            _tableName = environmentVariableGetter.Get("TABLE_NAME");
        }

        public async Task<QueryResponse> QueryAsync(QueryRequest request)
        {
            request.TableName = _tableName;
            return await _client.QueryAsync(request);
        }
    }
}
