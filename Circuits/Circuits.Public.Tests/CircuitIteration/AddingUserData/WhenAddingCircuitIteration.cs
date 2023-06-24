using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitIteration.AddingUserData
{
    public class WhenAddingCircuitIteration : CircuitIterationRepositoryTestBase
    {
        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a circuitId
            var circuitId = _faker.Random.Guid().ToString();

            // GIVEN DynamoDB is simulated
            CircuitIterationEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<CircuitIterationEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var result = await circuitIterationRepository.AddIterationAsync(authorizationHeader, circuitId);

            // THEN the circuit iteration should be saved to DynamoDB
            VerifyEntryWasSaved(userId, circuitId, savedEntry, result);

            // THEN the response should be the GUID of the new circuit iteration
            result.Should().Be(savedEntry?.IterationId);
            Guid.TryParse(result, out _).Should().BeTrue();
        }

        private void VerifyEntryWasSaved(string userId, string circuitId, CircuitIterationEntry? savedEntry, string result)
        {
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<CircuitIterationEntry>()), Times.Once);
            savedEntry?.CircuitIterationPointer.UserId.Should().Be(userId);
            savedEntry?.CircuitIterationPointer.CircuitId.Should().Be(circuitId);
            savedEntry?.IterationId.Should().Be(result);
            var savedDateStarted = DateTime.Parse(savedEntry?.DateStarted);
            savedDateStarted.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            savedEntry.DateCompleted.Should().Be(string.Empty);
        }
    }
}
