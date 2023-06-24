using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.Mockers;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingExercise : CircuitsRepositoryTestBase
    {
        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddExerciseRequest
            {
                Name = _faker.Random.AlphaNumeric(10),
                RepetitionType = _faker.Random.Enum<RepetitionType>(),
                DefaultEquipmentId = _faker.Random.Bool(0.5f) ? _faker.Random.Guid().ToString() : null,
            };

            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN DynamoDB is simulated
            ExerciseEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<ExerciseEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsRepository = BuildCircuitsRepository();
            var result = await circuitsRepository.AddExerciseAsync(authorizationHeader, request);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<ExerciseEntry>()), Times.Once);
            var expectedEntry = new ExerciseEntry
            {
                UserId = userId,
                ExerciseId = result,
                Name = request.Name,
                RepetitionType = request.RepetitionType,
                DefaultEquipmentId = request.DefaultEquipmentId
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Should().Be(savedEntry?.ExerciseId);
            Guid.TryParse(result, out _).Should().BeTrue();
        }
    }
}
