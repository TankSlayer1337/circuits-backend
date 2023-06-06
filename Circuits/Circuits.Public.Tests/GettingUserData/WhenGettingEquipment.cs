using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingEquipment
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

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, equipmentEntries);

            // WHEN getting equipment
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var results = await circuitsController.GetEquipment(new GetAllRequest { UserId = userId });

            // THEN the correct equipment representations are returned
            var expectedResults = equipmentEntries.Select(entry => new Equipment
            {
                Id = entry.EquipmentId,
                Name = entry.Name,
                CanBeUsedInMultiples = entry.CanBeUsedInMultiples
            });
            results.Value.Should().BeEquivalentTo(expectedResults);
        }
    }
}
