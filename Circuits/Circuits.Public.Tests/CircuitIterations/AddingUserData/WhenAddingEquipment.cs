using Circuits.Public.Controllers.Models.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitIterations.AddingUserData
{
    public class WhenAddingEquipment : CircuitIterationRepositoryTestBase
    {
        [Fact]
        public async void WithValidRequestAsync()
        {
            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN a request
            var request = new AddEquipmentInstanceRequest
            {
                CircuitId = _faker.Random.Guid().ToString(),
                IterationId = _faker.Random.Guid().ToString(),
                ItemId = _faker.Random.Guid().ToString(),
                OccurrenceId = _faker.Random.Guid().ToString(),
                SetId = _faker.Random.Guid().ToString(),
                EquipmentId = _faker.Random.Guid().ToString(),
                Count = _faker.Random.Int(1, 3),
                Load = _faker.Random.Float(1, 200),
                LoadUnit = _faker.PickRandom<LoadUnit>()
            };

            // GIVEN DynamoDB is simulated
            EquipmentInstanceEntry? savedEntry = null;
            _contextWrapperMocker.SimulateSaveAsync<EquipmentInstanceEntry>(item => savedEntry = item);

            // WHEN recording exercise
            var circuitIterationRepository = BuildCircuitIterationRepository();
            var equipmentInstanceId = await circuitIterationRepository.AddEquipmentInstance(authorizationHeader, request);

            // THEN an entry should be added to DynamoDB
            var expectedSavedEntry = GetExpectedEquipmentInstanceEntry(request, userId, equipmentInstanceId);
            savedEntry.Should().BeEquivalentTo(expectedSavedEntry);
        }

        public static EquipmentInstanceEntry GetExpectedEquipmentInstanceEntry(AddEquipmentInstanceRequest request, string userId, string equipmentInstanceId)
        {
            return new EquipmentInstanceEntry
            {
                CircuitIterationPointer = new CircuitIterationPointer
                {
                    CircuitId = request.CircuitId,
                    UserId = userId,
                    IterationId = request.IterationId
                },
                EquipmentInstancePointer = new EquipmentInstancePointer
                {
                    ItemId = request.ItemId,
                    OccurrenceId = request.OccurrenceId,
                    SetId = request.SetId,
                    EquipmentInstanceId = equipmentInstanceId
                },
                EquipmentId = request.EquipmentId,
                Count = request.Count,
                Load = request.Load,
                LoadUnit = (int)request.LoadUnit
            };
        }
    }
}
