using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingCircuit : CircuitsRepositoryTestBase
    {
        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a valid circuit name
            var name = _faker.Random.AlphaNumeric(10);

            // GIVEN DynamoDB is simulated
            CircuitEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<CircuitEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsRepository = BuildCircuitsRepository();
            var result = await circuitsRepository.AddCircuitAsync(authorizationHeader, name);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<CircuitEntry>()), Times.Once);
            var expectedEntry = new CircuitEntry
            {
                UserId = userId,
                CircuitId = result,
                Name = name
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Should().Be(savedEntry?.CircuitId);
            Guid.TryParse(result, out _).Should().BeTrue();
        }
    }
}
