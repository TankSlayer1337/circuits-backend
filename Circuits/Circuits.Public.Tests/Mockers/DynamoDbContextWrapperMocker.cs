using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB;

namespace Circuits.Public.Tests.Mockers
{
    public class DynamoDbContextWrapperMocker : Mocker<IDynamoDbContextWrapper>
    {
        public void SimulateSaveAsync<T>(Action<T> saveAction) where T : class
        {
            Mock.Setup(mock => mock.SaveAsync(It.IsAny<T>()))
                .Callback(saveAction)
                .Returns(Task.CompletedTask);
        }

        public void SimulateQueryAsync<T>(string hashKeyValue, QueryOperator queryOperator, IEnumerable<object> values, List<T> results) where T : class
        {
            Mock.Setup(mock => mock.QueryAsync<T>(hashKeyValue, queryOperator, values))
                .Returns(Task.FromResult(results));
        }

        public void SimulateQueryAsync<T>(object hashKeyValue, QueryOperator queryOperator, IEnumerable<object> values, List<T> results) where T : class
        {
            Mock.Setup(mock => mock.QueryAsync<T>(It.IsAny<object>(), queryOperator, values))
                .Callback<object, QueryOperator, IEnumerable<object>>((hashKey, _, _) =>
                {
                    hashKey.Should().BeEquivalentTo(hashKeyValue);
                })
                .Returns(Task.FromResult(results));
        }
    }
}
