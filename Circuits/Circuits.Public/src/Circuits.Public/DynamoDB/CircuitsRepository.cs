using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.Controllers.Models.GetRequests;
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
            var itemEntries = await _dynamoDbContext.QueryAsync<CircuitItem>(pointer, QueryOperator.BeginsWith, new string[] { string.Empty });
            var items = itemEntries.Select(entry => new CircuitItem
            {
                Index = entry.Index,
                OccurrenceWeight = entry.OccurrenceWeight,
                Exercise = entry.Exercise,
            })
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
    }
}
