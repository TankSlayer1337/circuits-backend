using Circuits.Public.Controllers.Models;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.AddingUserData
{
    public class WhenAddingEquipment
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            var request = new AddEquipmentRequest
            {
                UserId = _faker.Random.Guid().ToString(),
                Name = _faker.Random.AlphaNumeric(10),
                CanBeUsedInMultiples = _faker.Random.Bool()
            };

            // GIVEN DynamoDB is simulated
            EquipmentEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<EquipmentEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var result = await circuitsController.AddEquipment(request);

            // THEN the response should be the GUID of the new equipment
            result.Value.Should().Be(savedEntry?.EquipmentId);
            Guid.TryParse(result.Value, out _).Should().BeTrue();

            // THEN the equipment should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<EquipmentEntry>()), Times.Once);
            var expectedEntry = new EquipmentEntry
            {
                UserId = request.UserId,
                EquipmentId = result.Value,
                Name = request.Name,
                CanBeUsedInMultiples = request.CanBeUsedInMultiples
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);
        }
    }
}
