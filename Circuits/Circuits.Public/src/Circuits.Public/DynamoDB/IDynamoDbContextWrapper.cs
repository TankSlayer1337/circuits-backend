using Amazon.DynamoDBv2.DocumentModel;

namespace Circuits.Public.DynamoDB
{
    public interface IDynamoDbContextWrapper
    {
        Task SaveAsync<T>(T item) where T : class;
        Task<List<T>> QueryAsync<T>(object hashKeyValue, QueryOperator queryOperator, IEnumerable<object> values) where T : class;
        T FromDocument<T>(Document document);
    }
}