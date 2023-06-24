using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitDefinition.AddingUserData
{
    public class WhenAddingEquipment : CircuitsRepositoryTestBase
    {
        [Fact]
        public async Task WithValidRequestAsync()
        {
            // GIVEN a valid request
            AddEquipmentRequest addEquipmentRequest = BuildAddEquipmentRequest();

            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN DynamoDB is simulated
            EquipmentEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<EquipmentEntry>(item => savedEntry = item);

            // WHEN adding circuit
            var circuitsRepository = BuildCircuitsRepository();
            var result = await circuitsRepository.AddEquipmentAsync(authorizationHeader, addEquipmentRequest);

            // THEN the response should be the GUID of the new equipment
            result.Should().Be(savedEntry?.EquipmentId);
            Guid.TryParse(result, out _).Should().BeTrue();

            // THEN the equipment should be saved to DynamoDB
            _contextWrapperMocker.Mock.Verify(mock => mock.SaveAsync(It.IsAny<EquipmentEntry>()), Times.Once);
            var expectedEntry = new EquipmentEntry
            {
                UserId = userId,
                EquipmentId = result,
                Name = addEquipmentRequest.Name,
                CanBeUsedInMultiples = addEquipmentRequest.CanBeUsedInMultiples
            };
            savedEntry.Should().BeEquivalentTo(expectedEntry);
        }

        private AddEquipmentRequest BuildAddEquipmentRequest()
        {
            return new AddEquipmentRequest
            {
                Name = _faker.Random.AlphaNumeric(10),
                CanBeUsedInMultiples = _faker.Random.Bool()
            };
        }
    }
}
