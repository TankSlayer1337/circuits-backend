using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingCircuits
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async void WithSuccess()
        {
            // GIVEN a UserId
            var userId = _faker.Random.Guid().ToString();

            // GIVEN user has circuit entries
            var circuitEntries = CrreateRandomCircuitEntries(userId);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, circuitEntries);

            // WHEN geting equipment
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var results = await circuitsController.GetCircuits(new GetAllRequest { UserId = userId });

            // THEN the correct circuit representations are returned
            var expectedResults = circuitEntries.Select(entry => new Circuit
            {
                Id = entry.CircuitId,
                Name = entry.Name
            });
            results.Value.Should().BeEquivalentTo(expectedResults);
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
