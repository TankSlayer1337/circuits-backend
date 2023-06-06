using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingItems
    {
        private readonly Faker _faker = new();
        private readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();

        [Fact]
        public async void WithSuccess()
        {
            // GIVEN a UserId and CircuitId
            var userId = _faker.Random.Guid().ToString();
            var circuitId = _faker.Random.Guid().ToString();

            // GIVEN user has equipment entries
            var equipmentEntries = RandomCreator.CreateEquipmentEntries(userId);

            // GIVEN user has exercise entries
            var exerciseEntries = RandomCreator.CreateExerciseEntries(userId, equipmentEntries);

            // GIVEN user has item entries
            var pointer = new CircuitItemPointer { UserId = userId, CircuitId = circuitId };
            var itemEntries = CreateItemEntries(pointer, exerciseEntries);

            // GIVEN DynamoDB is simulated
            _contextWrapperMocker.SimulateQueryAsync(pointer, QueryOperator.BeginsWith, new string[] { string.Empty }, itemEntries);
            foreach(var exerciseEntry in exerciseEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { exerciseEntry.ExerciseId }, new List<ExerciseEntry>() { exerciseEntry });
            }
            foreach(var equipmentEntry in equipmentEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { equipmentEntry.EquipmentId }, new List<EquipmentEntry>() { equipmentEntry });
            }

            // WHEN getting items
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var results = await circuitsController.GetCircuitItems(new GetItemsRequest { UserId = userId, CircuitId = circuitId });

            // THEN the correct item representations are returned
            IEnumerable<Item> expectedResults = CreateItems(itemEntries, exerciseEntries, equipmentEntries);
            results.Value.Should().BeEquivalentTo(expectedResults);
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
