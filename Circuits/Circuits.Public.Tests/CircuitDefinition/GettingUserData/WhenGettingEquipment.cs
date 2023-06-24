using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitDefinition.GettingUserData
{
    public class WhenGettingEquipment : CircuitsRepositoryTestBase
    {
        [Fact]
        public async void WithSuccess()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN user has equipment entries
            var equipmentEntries = RandomCreator.CreateEquipmentEntries(userId);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, equipmentEntries);

            // WHEN getting equipment
            var circuitsRepository = BuildCircuitsRepository();
            var results = await circuitsRepository.GetEquipmentAsync(authorizationHeader);

            // THEN the correct equipment representations are returned
            var expectedResults = equipmentEntries.Select(entry => new Equipment
            {
                Id = entry.EquipmentId,
                Name = entry.Name,
                CanBeUsedInMultiples = entry.CanBeUsedInMultiples
            });
            results.Should().BeEquivalentTo(expectedResults);
        }
    }
}
