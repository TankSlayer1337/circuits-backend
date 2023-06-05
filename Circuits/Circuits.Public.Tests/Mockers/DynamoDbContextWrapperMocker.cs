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
    }
}
