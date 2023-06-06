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

        public void SimulateQueryAsync<T>(object hashKeyValue, QueryOperator queryOperator, object value, List<T> results) where T : class
        {
            Mock.Setup(mock => mock.QueryAsync<T>(hashKeyValue, queryOperator, It.IsAny<IEnumerable<object>>()))
                .Callback<object, QueryOperator, IEnumerable<object>>((_, _, values) =>
                {
                    values.Should().BeEquivalentTo(new object[] { value });
                })
                .Returns(Task.FromResult(results));
        }
    }
}
