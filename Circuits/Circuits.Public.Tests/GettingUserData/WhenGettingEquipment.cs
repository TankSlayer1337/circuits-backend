using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
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
            var equipmentEntries = CreateRandomEquipmentEntries(userId);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, string.Empty, equipmentEntries);

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

        private List<EquipmentEntry> CreateRandomEquipmentEntries(string userId)
        {
            var equipmentEntries = new List<EquipmentEntry>();
            var entriesCount = _faker.Random.Int(1, 10);
            for (var i = 0; i < entriesCount; i++)
            {
                equipmentEntries.Add(new EquipmentEntry
                {
                    UserId = userId,
                    EquipmentId = _faker.Random.Guid().ToString(),
                    Name = _faker.Random.AlphaNumeric(10),
                    CanBeUsedInMultiples = _faker.Random.Bool()
                });
            }
            return equipmentEntries;
        }
    }
}
