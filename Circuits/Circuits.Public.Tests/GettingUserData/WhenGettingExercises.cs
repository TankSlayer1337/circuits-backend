using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingExercises : CircuitsRepositoryTestBase
    {
        [Fact]
        public async void WithSuccess()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

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
            var circuitsRepository = BuildCircuitsRepository();
            var results = await circuitsRepository.GetExercisesAsync(authorizationHeader);

            // THEN the correct exercise representations are returned
            IEnumerable<Exercise> expectedResults = DataCreator.CreateExercises(equipmentEntries, exerciseEntries);
            results.Should().BeEquivalentTo(expectedResults);
        }
    }
}
