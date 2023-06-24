using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitDefinition.GettingUserData
{
    public class WhenGettingCircuits : CircuitsRepositoryTestBase
    {
        [Fact]
        public async void WithSuccess()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN user has circuit entries
            var circuitEntries = CrreateRandomCircuitEntries(userId);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, circuitEntries);

            // WHEN geting equipment
            var circuitsRepository = BuildCircuitsRepository();
            var results = await circuitsRepository.GetCircuitsAsync(authorizationHeader);

            // THEN the correct circuit representations are returned
            var expectedResults = circuitEntries.Select(entry => new Circuit
            {
                Id = entry.CircuitId,
                Name = entry.Name
            });
            results.Should().BeEquivalentTo(expectedResults);
        }

        private List<CircuitEntry> CrreateRandomCircuitEntries(string userId)
        {
            var circuitEntries = new List<CircuitEntry>();
            var entriesCount = _faker.Random.Int(1, 10);
            for (var i = 0; i < entriesCount; i++)
            {
                circuitEntries.Add(new CircuitEntry
                {
                    UserId = userId,
                    CircuitId = _faker.Random.Guid().ToString(),
                    Name = _faker.Random.AlphaNumeric(10)
                });
            }
            return circuitEntries;
        }
    }
}
