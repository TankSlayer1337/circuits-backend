using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingExercises
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async void WithSuccess()
        {
            // GIVEN a UserId
            var userId = _faker.Random.Guid().ToString();

            // GIVEN user has equipment entries
            var equipmentEntries = RandomCreator.CreateEquipmentEntries(userId);

            // GIVEN user has exercise entries
            var exerciseEntries = RandomCreator.CreateExerciseEntries(userId, equipmentEntries);

            // GIVEN DynamoDB is simulated
            foreach(var entry in equipmentEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { entry.EquipmentId }, new List<EquipmentEntry> { entry });
            }
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, exerciseEntries);

            // WHEN getting exercises
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var results = await circuitsController.GetExercises(new GetAllRequest { UserId = userId });

            // THEN the correct exercise representations are returned
            IEnumerable<Exercise> expectedResults = DataCreator.CreateExercises(equipmentEntries, exerciseEntries);
            results.Value.Should().BeEquivalentTo(expectedResults);
        }
    }
}
