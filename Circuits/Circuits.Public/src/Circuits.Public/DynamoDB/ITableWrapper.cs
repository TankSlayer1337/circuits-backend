using Amazon.DynamoDBv2.DocumentModel;

namespace Circuits.Public.DynamoDB
{
    public interface ITableWrapper
    {
        Search Query(Primitive hashKey, QueryFilter filter);
    }
}