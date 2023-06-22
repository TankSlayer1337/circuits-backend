using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingItem : CircuitsRepositoryTestBase
    {
        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddItemRequest
            {
                CircuitId = _faker.Random.Guid().ToString(),
                Index = _faker.Random.UInt(),
                ExerciseId = _faker.Random.Guid().ToString(),
                OccurrenceWeight = _faker.Random.UInt()
            };

            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN DynamoDB is simulated
            ItemEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<ItemEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsRepository = BuildCircuitsRepository();
            var result = await circuitsRepository.AddItemAsync(authorizationHeader, request);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<ItemEntry>()), Times.Once);
            var expectedEntry = new ItemEntry
            {
                Pointer = new CircuitItemPointer
                {
                    UserId = userId,
                    CircuitId = request.CircuitId
                },
                ItemId = result,
                Index = request.Index,
                ExerciseId = request.ExerciseId,
                OccurrenceWeight = request.OccurrenceWeight
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Should().Be(savedEntry?.ItemId);
            Guid.TryParse(result, out _).Should().BeTrue();
        }
    }
}
