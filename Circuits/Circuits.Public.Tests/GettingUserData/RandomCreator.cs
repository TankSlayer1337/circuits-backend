using Circuits.Public.DynamoDB.Models.ExerciseCircuit;

namespace Circuits.Public.Tests.GettingUserData
{
    internal class RandomCreator
    {
        public static List<EquipmentEntry> CreateEquipmentEntries(string userId)
        {
            var faker = new Faker();
            var equipmentEntries = new List<EquipmentEntry>();
            var entriesCount = faker.Random.Int(1, 10);
            for (var i = 0; i < entriesCount; i++)
            {
                equipmentEntries.Add(new EquipmentEntry
                {
                    UserId = userId,
                    EquipmentId = faker.Random.Guid().ToString(),
                    Name = faker.Random.AlphaNumeric(10),
                    CanBeUsedInMultiples = faker.Random.Bool()
                });
            }
            return equipmentEntries;
        }
    }
}
