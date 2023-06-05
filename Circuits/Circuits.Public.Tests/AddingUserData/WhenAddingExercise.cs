using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingExercise
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddExerciseRequest
            {
                UserId = _faker.Random.Guid().ToString(),
                Name = _faker.Random.AlphaNumeric(10),
                RepetitionType = _faker.Random.Enum<RepetitionType>(),
                DefaultEquipmentId = _faker.Random.Bool(0.5f) ? _faker.Random.Guid().ToString() : null,
            };

            // GIVEN DynamoDB is simulated
            ExerciseEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<ExerciseEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var result = await circuitsController.AddExercise(request);

            // THEN the circuit should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<ExerciseEntry>()), Times.Once);
            var expectedEntry = new ExerciseEntry
            {
                UserId = request.UserId,
                ExerciseId = result.Value,
                Name = request.Name,
                RepetitionType = request.RepetitionType,
                DefaultEquipmentId = request.DefaultEquipmentId
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);

            // THEN the response should be the GUID of the new circuit
            result.Value.Should().Be(savedEntry?.ExerciseId);
            Guid.TryParse(result.Value, out _).Should().BeTrue();
        }
    }
}
