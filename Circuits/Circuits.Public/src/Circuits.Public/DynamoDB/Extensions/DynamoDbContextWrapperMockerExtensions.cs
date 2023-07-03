using Amazon.DynamoDBv2.DocumentModel;

namespace Circuits.Public.DynamoDB.Extensions
{
    public static class DynamoDbContextWrapperMockerExtensions
    {
        public static Task<List<T>> QueryWithEmptyBeginsWith<T>(this IDynamoDbContextWrapper dynamoDbContext, object hashKeyValue) where T : class
        {
            return dynamoDbContext.QueryAsync<T>(hashKeyValue, QueryOperator.BeginsWith, new string[] { string.Empty });
        }
    }
}
