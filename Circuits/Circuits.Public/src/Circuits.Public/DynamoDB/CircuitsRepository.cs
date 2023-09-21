using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.DynamoDB.Extensions;
using Circuits.Public.DynamoDB.Models.CircuitDefinition;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Circuits.Public.UserInfo;

namespace Circuits.Public.DynamoDB
{
    // TODO: replace usage with interface
    public class CircuitsRepository
    {
        private readonly IDynamoDbContextWrapper _dynamoDbContext;
        private readonly IUserInfoGetter _userInfoGetter;

        public CircuitsRepository(IDynamoDbContextWrapper dynamoDbContext, IUserInfoGetter userInfoGetter)
        {
            _dynamoDbContext = dynamoDbContext;
            _userInfoGetter = userInfoGetter;
        }

        public async Task<string> AddCircuitAsync(string authorizationHeader, string name)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var circuitEntry = CircuitEntry.Create(userId, name);
            await _dynamoDbContext.SaveAsync(circuitEntry);
            return circuitEntry.CircuitId;
        }

        public async Task<List<Circuit>> GetCircuitsAsync(string authorizationHeader)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var circuitEntries = await _dynamoDbContext.QueryWithEmptyBeginsWith<CircuitEntry>(userId);
            var circuits = circuitEntries.Select(entry => new Circuit
            {
                Id = entry.CircuitId,
                Name = entry.Name
            });
            return circuits.ToList();
        }

        public async Task<string> AddItemAsync(string authorizationHeader, AddItemRequest addItemRequest)
        {
            // TODO: validate the request, e.g. does the ExerciseId exist?
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var itemEntry = ItemEntry.FromRequest(userId, addItemRequest);
            await _dynamoDbContext.SaveAsync(itemEntry);
            return itemEntry.ItemId;
        }

        public async Task<List<Item>> GetItemsAsyncWithAuthorizationHeader(string authorizationHeader, string circuitId)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            return await GetItemsAsyncWithUserId(userId, circuitId);
        }

        public async Task<List<Item>> GetItemsAsyncWithUserId(string userId, string circuitId)
        {
            var pointer = new CircuitItemPointer
            {
                UserId = userId,
                CircuitId = circuitId
            };
            var itemEntries = await _dynamoDbContext.QueryWithEmptyBeginsWith<ItemEntry>(pointer);
            var items = new List<Item>();
            foreach (var entry in itemEntries)
            {
                var exercise = await GetExerciseAsync(userId, entry.ExerciseId);
                var item = new Item
                {
                    ItemId = entry.ItemId,
                    Index = entry.Index,
                    OccurrenceWeight = entry.OccurrenceWeight,
                    Exercise = exercise
                };
                items.Add(item);
            }
            var sortedItems = items.OrderBy(item => item.Index).ToList();
            return sortedItems;
        }

        public async Task<string> AddExerciseAsync(string authorizationHeader, AddExerciseRequest addExerciseRequest)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var exerciseEntry = ExerciseEntry.FromRequest(userId, addExerciseRequest);
            await _dynamoDbContext.SaveAsync(exerciseEntry);
            return exerciseEntry.ExerciseId;
        }

        public async Task<List<Exercise>> GetExercisesAsync(string authorizationHeader)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var exerciseEntries = await _dynamoDbContext.QueryWithEmptyBeginsWith<ExerciseEntry>(userId);
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

        public async Task<string> AddEquipmentAsync(string authorizationHeader, AddEquipmentRequest addEquipmentRequest)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var equipmentEntry = EquipmentEntry.FromRequest(userId, addEquipmentRequest);
            await _dynamoDbContext.SaveAsync(equipmentEntry);
            return equipmentEntry.EquipmentId;
        }

        public async Task<List<Equipment>> GetEquipmentAsync(string authorizationHeader)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var equipmentEntries = await _dynamoDbContext.QueryWithEmptyBeginsWith<EquipmentEntry>(userId);
            var equipment = equipmentEntries.Select(entry => new Equipment
            {
                Id = entry.EquipmentId,
                Name = entry.Name,
                CanBeUsedInMultiples = entry.CanBeUsedInMultiples
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
