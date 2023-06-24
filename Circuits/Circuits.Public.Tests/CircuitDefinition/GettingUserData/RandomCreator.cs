using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.Tests.CircuitDefinition.GettingUserData
{
    internal class RandomCreator
    {
        private static readonly Faker _faker = new();

        public static List<EquipmentEntry> CreateEquipmentEntries(string userId)
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

        public static List<ExerciseEntry> CreateExerciseEntries(string userId, List<EquipmentEntry> equipmentEntries)
        {
            var exerciseEntries = new List<ExerciseEntry>();
            var entryCount = _faker.Random.Int(1, 10);
            for (var i = 0; i < entryCount; i++)
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
