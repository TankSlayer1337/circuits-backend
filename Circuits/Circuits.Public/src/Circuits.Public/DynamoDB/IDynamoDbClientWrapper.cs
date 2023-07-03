using Amazon.DynamoDBv2.Model;

namespace Circuits.Public.DynamoDB
{
    public interface IDynamoDbClientWrapper
    {
        Task<QueryResponse> QueryAsync(QueryRequest request);
    }
}