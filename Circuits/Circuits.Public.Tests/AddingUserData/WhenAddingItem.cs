using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingItem
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddItemRequest
            {
                UserId = _faker.Random.Guid().ToString(),
                CircuitId = _faker.Random.Guid().ToString(),
                Index = _faker.Random.UInt(),
                ExerciseId = _faker.Random.Guid().ToString(),
                OccurrenceWeight = _faker.Random.UInt()
            };

            // GIVEN DynamoDB is simulated
            CircuitItemEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<CircuitItemEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var result = await circuitsController.AddCircuitItem(request);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<CircuitItemEntry>()), Times.Once);
            var expectedEntry = new CircuitItemEntry
            {
                Pointer = new CircuitItemPointer
                {
                    UserId = request.UserId,
                    CircuitId = request.CircuitId
                },
                ItemId = result.Value,
                Index = request.Index,
                ExerciseId = request.ExerciseId,
                OccurrenceWeight = request.OccurrenceWeight
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Value.Should().Be(savedEntry?.ItemId);
            Guid.TryParse(result.Value, out _).Should().BeTrue();
        }
    }
}
