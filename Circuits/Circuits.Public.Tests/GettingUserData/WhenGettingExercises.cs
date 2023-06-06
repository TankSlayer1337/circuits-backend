using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.Tests.AddingUserData;
using Circuits.Public.Tests.Mockers;

namespace Circuits.Public.Tests.GettingUserData
{
    public class WhenGettingExercises
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

            // GIVEN user has exercise entries
            var exerciseEntries = CreateRandomExerciseEntries(userId, equipmentEntries);

            // GIVEN DynamoDB is simulated
            foreach(var entry in equipmentEntries)
            {
                _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.Equal, new string[] { entry.EquipmentId }, new List<EquipmentEntry> { entry });
            }
            _contextWrapperMocker.SimulateQueryAsync(userId, QueryOperator.BeginsWith, new string[] { string.Empty }, exerciseEntries);

            // WHEN getting exercises
            var circuitsController = TestHelper.BuildCircuitsController(_contextWrapperMocker);
            var results = await circuitsController.GetExercises(new GetAllRequest { UserId = userId });

            // THEN the correct exercise representations are returned
            IEnumerable<Exercise> expectedResults = CreateExercises(equipmentEntries, exerciseEntries);
            results.Value.Should().BeEquivalentTo(expectedResults);
        }

        private static IEnumerable<Exercise> CreateExercises(List<EquipmentEntry> equipmentEntries, List<ExerciseEntry> exerciseEntries)
        {
            return exerciseEntries.Select(entry =>
            {
                Equipment? defaultEquipment = null;
                if (!string.IsNullOrEmpty(entry.DefaultEquipmentId))
                {
                    var equipmentEntry = equipmentEntries.Single(equipment => equipment.EquipmentId == entry.DefaultEquipmentId);
                    defaultEquipment = new Equipment
                    {
                        Id = equipmentEntry.EquipmentId,
                        Name = equipmentEntry.Name,
                        CanBeUsedInMultiples = equipmentEntry.CanBeUsedInMultiples
                    };
                }
                return new Exercise
                {
                    Id = entry.ExerciseId,
                    Name = entry.Name,
                    RepetitionType = entry.RepetitionType,
                    DefaultEquipment = defaultEquipment
                };
            });
        }

        private List<ExerciseEntry> CreateRandomExerciseEntries(string userId, List<EquipmentEntry> equipmentEntries)
        {
            var exerciseEntries = new List<ExerciseEntry>();
            var entriesCount = _faker.Random.Int(1, 10);
            for (var i = 0;  i < entriesCount; i++)
            {
                exerciseEntries.Add(new ExerciseEntry
                {
                    UserId = userId,
                    ExerciseId = _faker.Random.Guid().ToString(),
                    Name = _faker.Random.AlphaNumeric(10),
                    RepetitionType = _faker.Random.Enum<RepetitionType>(),
                    DefaultEquipmentId = _faker.Random.Bool(0.5f) ? null : _faker.PickRandom(equipmentEntries).EquipmentId,
                });
            }
            return exerciseEntries;
        }
    }
}
