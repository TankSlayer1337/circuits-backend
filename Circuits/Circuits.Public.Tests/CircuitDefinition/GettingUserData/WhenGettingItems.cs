using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.CircuitDefinition.AddingUserData;
using Circuits.Public.Tests.Utils;

namespace Circuits.Public.Tests.CircuitDefinition.GettingUserData
{
    public class WhenGettingItems : CircuitsRepositoryTestBase
    {
        [Fact]
        public async void WithSuccess()
        {
            // GIVEN a CircuitId
            var circuitId = _faker.Random.Guid().ToString();

            // GIVEN UserInfo endpoint is simulated
            var (userId, authorizationHeader) = UserInfoEndpointSimulator.SimulateUserInfoEndpoint(_httpClientWrapperMocker, _environmentVariableGetterMocker);

            // GIVEN user has equipment entries
            var equipmentEntries = RandomCreator.CreateEquipmentEntries(userId);

            // GIVEN user has exercise entries
            var exerciseEntries = RandomCreator.CreateExerciseEntries(userId, equipmentEntries);

            // GIVEN user has item entries
            var pointer = new CircuitItemPointer { UserId = userId, CircuitId = circuitId };
            var itemEntries = CreateItemEntries(pointer, exerciseEntries);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(pointer, QueryOperator.BeginsWith, new string[] { string.Empty }, itemEntries);
            foreach (var exerciseEntry in exerciseEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { exerciseEntry.ExerciseId }, new List<ExerciseEntry>() { exerciseEntry });
            }
            foreach (var equipmentEntry in equipmentEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { equipmentEntry.EquipmentId }, new List<EquipmentEntry>() { equipmentEntry });
            }

            // WHEN getting items
            var circuitsRepository = BuildCircuitsRepository();
            var results = await circuitsRepository.GetItemsAsync(authorizationHeader, circuitId);

            // THEN the correct item representations are returned
            IEnumerable<Item> expectedResults = CreateItems(itemEntries, exerciseEntries, equipmentEntries);
            results.Should().BeEquivalentTo(expectedResults);
        }

        private IEnumerable<Item> CreateItems(List<ItemEntry> itemEntries, List<ExerciseEntry> exerciseEntries, List<EquipmentEntry> equipmentEntries)
        {
            var exercises = DataCreator.CreateExercises(equipmentEntries, exerciseEntries);
            return itemEntries.Select(entry => new Item
            {
                ItemId = entry.ItemId,
                Index = entry.Index,
                OccurrenceWeight = entry.OccurrenceWeight,
                Exercise = exercises.Single(exercise => exercise.Id == entry.ExerciseId)
            });
        }

        private List<ItemEntry> CreateItemEntries(CircuitItemPointer pointer, List<ExerciseEntry> exerciseEntries)
        {
            var itemEntries = new List<ItemEntry>();
            var entryCount = _faker.Random.Int(1, 10);
            for (var i = 0; i < entryCount; i++)
            {
                itemEntries.Add(new ItemEntry
                {
                    Pointer = pointer,
                    ItemId = _faker.Random.Guid().ToString(),
                    Index = (uint)i,
                    ExerciseId = _faker.PickRandom(exerciseEntries).ExerciseId,
                    OccurrenceWeight = (uint)_faker.Random.Int(1, 5)
                });
            }
            return itemEntries;
        }
    }
}
