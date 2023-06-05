namespace Circuits.Public.DynamoDB
{
    public interface IDynamoDbContextWrapper
    {
        Task SaveAsync<T>(T item) where T : class;
    }
}