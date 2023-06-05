using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.DynamoDB
{
    public class CircuitsRepository
    {
        private readonly IDynamoDbContextWrapper _dynamoDbContext;

        public CircuitsRepository(IDynamoDbContextWrapper dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<string> AddCircuitAsync(string userId, string name)
        {
            var circuitEntry = new ExerciseCircuitEntry
            {
                UserId = userId,
                CircuitId = Guid.NewGuid().ToString(),
                Name = name
            };
            await _dynamoDbContext.SaveAsync(circuitEntry);
            return circuitEntry.CircuitId;
        }

        public async Task<List<ExerciseCircuit>> GetCircuitsAsync(string userId)
        {
            var circuitEntries = await _dynamoDbContext.QueryAsync<ExerciseCircuitEntry>(userId, QueryOperator.BeginsWith, new string[] { string.Empty });
            var circuits = circuitEntries.Select(entry => new ExerciseCircuit
            {
                Id = entry.CircuitId,
                Name = entry.Name
            });
            return circuits.ToList();
        }

        public async Task<string> AddItemAsync(AddItemRequest request)
        {
            // TODO: validate the request, e.g. does the ExerciseId exist?
            var pointer = new CircuitItemPointer
            {
                UserId = request.UserId,
                CircuitId = request.CircuitId
            };
            var itemEntry = new CircuitItemEntry
            {
                Pointer = pointer,
                ItemId = Guid.NewGuid().ToString(),
                Index = request.Index,
                ExerciseId = request.ExerciseId,
                OccurrenceWeight = request.OccurrenceWeight
            };
            await _dynamoDbContext.SaveAsync(itemEntry);
            return itemEntry.ItemId;
        }

        public async Task<List<CircuitItem>> GetItemsAsync(string userId, string circuitId)
        {
            var pointer = new CircuitItemPointer
            {
                UserId = userId,
                CircuitId = circuitId
            };
            var itemEntries = await _dynamoDbContext.QueryAsync<CircuitItemEntry>(pointer, QueryOperator.BeginsWith, new string[] { string.Empty });
            var items = new List<CircuitItem>();
            foreach (var entry in itemEntries)
            {
                var exercise = await GetExerciseAsync(userId, entry.ExerciseId);
                var item = new CircuitItem
                {
                    Index = entry.Index,
                    OccurrenceWeight = entry.OccurrenceWeight,
                    Exercise = exercise
                };
                items.Add(item);
            }
            var sortedItems = items.OrderBy(item => item.Index).ToList();
            return sortedItems;
        }

        public async Task<string> AddExerciseAsync(AddExerciseRequest request)
        {
            var exerciseEntry = new ExerciseEntry
            {
                UserId = request.UserId,
                ExerciseId = Guid.NewGuid().ToString(),
                Name = request.Name,
                RepetitionType = request.RepetitionType,
                DefaultEquipmentId = request.DefaultEquipmentId
            };
            await _dynamoDbContext.SaveAsync(exerciseEntry);
            return exerciseEntry.ExerciseId;
        }

        public async Task<List<Exercise>> GetExercisesAsync(string userId)
        {
            var exerciseEntries = await _dynamoDbContext.QueryAsync<ExerciseEntry>(userId, QueryOperator.BeginsWith, new string[] { string.Empty });
            var exercises = new List<Exercise>();
            foreach (var entry in exerciseEntries)
            {
                var defaultEquipment = string.IsNullOrEmpty(entry.DefaultEquipmentId) ? null : await GetEquipmentAsync(userId, entry.DefaultEquipmentId);
                var exercise = new Exercise
                {
                    Id = entry.ExerciseId,
                    Name = entry.Name,
                    RepetitionType = entry.RepetitionType,
                    DefaultEquipment = defaultEquipment
                };
                exercises.Add(exercise);
            };
            return exercises;
        }

        private async Task<Exercise> GetExerciseAsync(string userId, string exerciseId)
        {
            var exerciseEntries = await _dynamoDbContext.QueryAsync<ExerciseEntry>(userId, QueryOperator.Equal, new string[] { exerciseId });
            var exercise = exerciseEntries.Single();
            var defaultEquipment = string.IsNullOrEmpty(exercise.DefaultEquipmentId) ? null : await GetEquipmentAsync(userId, exercise.DefaultEquipmentId);
            return new Exercise
            {
                Id = exercise.ExerciseId,
                Name = exercise.Name,
                RepetitionType = exercise.RepetitionType,
                DefaultEquipment = defaultEquipment
            };
        }

        public async Task<string> AddEquipmentAsync(AddEquipmentRequest request)
        {
            var equipmentEntry = new EquipmentEntry
            {
                UserId = request.UserId,
                EquipmentId = Guid.NewGuid().ToString(),
                Name = request.Name,
                CanBeUsedInMultiples = request.CanBeUsedInMultiples
            };
            await _dynamoDbContext.SaveAsync(equipmentEntry);
            return equipmentEntry.EquipmentId;
        }

        public async Task<List<Equipment>> GetEquipmentAsync(string userId)
        {
            var equipmentEntries = await _dynamoDbContext.QueryAsync<EquipmentEntry>(userId, QueryOperator.BeginsWith, new string[] { string.Empty });
            var equipment = equipmentEntries.Select(entry => new Equipment
            {
                Id = entry.EquipmentId,
                Name = entry.Name,
            });
            return equipment.ToList();
        }

        private async Task<Equipment> GetEquipmentAsync(string userId, string equipmentId)
        {
            var equipmentEntries = await _dynamoDbContext.QueryAsync<EquipmentEntry>(userId, QueryOperator.Equal, new string[] { equipmentId });
            var equipmentEntry = equipmentEntries.Single();
            return new Equipment
            {
                Id = equipmentEntry.EquipmentId,
                Name = equipmentEntry.Name,
                CanBeUsedInMultiples = equipmentEntry.CanBeUsedInMultiples
            };
        }
    }
}
