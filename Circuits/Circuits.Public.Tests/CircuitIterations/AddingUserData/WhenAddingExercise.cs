using Circuits.Public.Controllers.Models.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitIterations.AddingUserData
{
    public class WhenAddingExercise : CircuitIterationRepositoryTestBase
    {
        [Fact]
        public async void WithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a request
            var request = new AddRecordedExerciseRequest
            {
                CircuitId = _faker.Random.Guid().ToString(),
                IterationId = _faker.Random.Guid().ToString(),
                ItemId = _faker.Random.Guid().ToString(),
                ExerciseId = _faker.Random.Guid().ToString()
            };

            // GIVEN DynamoDB is simulated
            RecordedExerciseEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<RecordedExerciseEntry>(item => savedEntry = item);

            // WHEN recording exercise
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var occurrenceId = await circuitIterationRepository.AddRecordedExercise(authorizationHeader, request);

            // THEN an entry should be added to DynamoDB
            var expectedSavedEntry = GetExpectedRecordedExerciseEntry(request, userId, occurrenceId);
            savedEntry.Should().BeEquivalentTo(expectedSavedEntry);
        }

        private RecordedExerciseEntry GetExpectedRecordedExerciseEntry(AddRecordedExerciseRequest request, string userId, string occurrenceId)
        {
            return new RecordedExerciseEntry
            {
                CircuitIterationPointer = new CircuitIterationPointer
                {
                    CircuitId = request.CircuitId,
                    UserId = userId,
                    IterationId = request.IterationId
                },
                RecordedExercisePointer = new RecordedExercisePointer
                {
                    ItemId = request.ItemId,
                    OccurrenceId = occurrenceId
                },
                ExerciseId = request.ExerciseId
            };
        }
    }
}
