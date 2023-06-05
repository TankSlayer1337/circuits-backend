using Circuits.Public.Controllers.Models;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingCircuit
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddCircuitRequest
            {
                UserId = _faker.Random.Guid().ToString(),
                Name = _faker.Random.AlphaNumeric(10)
            };

            // GIVEN DynamoDB is simulated
            ExerciseCircuitEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<ExerciseCircuitEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var result = await circuitsController.AddCircuit(request);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<ExerciseCircuitEntry>()), Times.Once);
            var expectedEntry = new ExerciseCircuitEntry
            {
                UserId = request.UserId,
                CircuitId = result.Value,
                Name = request.Name
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Value.Should().Be(savedEntry?.CircuitId);
            Guid.TryParse(result.Value, out _).Should().BeTrue();
        }
    }
}
