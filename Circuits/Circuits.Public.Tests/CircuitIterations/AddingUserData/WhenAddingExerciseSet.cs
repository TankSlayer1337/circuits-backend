using Circuits.Public.Controllers.Models.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitIterations.AddingUserData
{
    public class WhenAddingExerciseSet : CircuitIterationRepositoryTestBase
    {
        [Fact]
        public async void WithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a request
            var request = new AddExerciseSetRequest
            {
                CircuitId = _faker.Random.Guid().ToString(),
                IterationId = _faker.Random.Guid().ToString(),
                ItemId = _faker.Random.Guid().ToString(),
                OccurrenceId = _faker.Random.Guid().ToString(),
                RepetitionMeasurement = _faker.Random.Int(1, 20).ToString(),
                SetIndex = _faker.Random.Int(1, 5),
                RepetitionType = _faker.PickRandom<RepetitionType>()
            };

            // GIVEN DynamoDB is simulated
            ExerciseSetEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<ExerciseSetEntry>(item => savedEntry = item);

            // WHEN recording exercise
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var setId = await circuitIterationRepository.AddExerciseSet(authorizationHeader, request);

            // THEN an entry should be added to DynamoDB
            var expectedSavedEntry = GetExpectedExerciseSetEntry(request, userId, setId);
            savedEntry.Should().BeEquivalentTo(expectedSavedEntry);
        }

        private ExerciseSetEntry GetExpectedExerciseSetEntry(AddExerciseSetRequest request, string userId, string setId)
        {
            return new ExerciseSetEntry
            {
                CircuitIterationPointer = new CircuitIterationPointer
                {
                    CircuitId = request.CircuitId,
                    UserId = userId,
                    IterationId = request.IterationId
                },
                ExerciseSetPointer = new ExerciseSetPointer
                {
                    ItemId = request.ItemId,
                    OccurrenceId = request.OccurrenceId,
                    SetId = setId
                },
                SetIndex = request.SetIndex,
                RepetitionType = request.RepetitionType,
                RepetitionMeasurement = request.RepetitionMeasurement
            };
        }
    }
}
