using Circuits.Public.DynamoDB;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;

namespace Circuits.Public.Tests.Mockers
{
    public class TableQuerierMocker : Mocker<ITableQuerier>
    {
        public void SimulateRunIterationQueryAsync(CircuitIterationPointer expectedPointer, IterationQueryResult result)
        {
            Mock.Setup(mock => mock.RunIterationQueryAsync(It.IsAny<CircuitIterationPointer>()))
                .Callback<CircuitIterationPointer>(circuitIterationPointer =>
                {
                    circuitIterationPointer.Should().BeEquivalentTo(expectedPointer);
                })
                .Returns(Task.FromResult(result));
        }
    }
}
