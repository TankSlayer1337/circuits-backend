using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Utilities;

namespace Circuits.Public.DynamoDB
{
    public class TableWrapper : ITableWrapper
    {
        private readonly Table _table;

        public TableWrapper(AmazonDynamoDBClient client, IEnvironmentVariableGetter environmentVariableGetter)
        {
            var tableName = environmentVariableGetter.Get("TABLE_NAME");
            _table = Table.LoadTable(client, tableName);
        }

        public Search Query(Primitive hashKey, QueryFilter filter)
        {
            return _table.Query(hashKey, filter);
        }
    }
}
