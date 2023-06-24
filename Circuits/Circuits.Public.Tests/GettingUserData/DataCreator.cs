using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.Tests.GettingUserData
{
    internal class DataCreator
    {
        public static IEnumerable<Exercise> CreateExercises(List<EquipmentEntry> equipmentEntries, List<ExerciseEntry> exerciseEntries)
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
    }
}
