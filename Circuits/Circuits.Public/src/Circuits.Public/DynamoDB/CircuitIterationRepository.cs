using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Extensions;
using Circuits.Public.DynamoDB.Models;
using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Circuits.Public.UserInfo;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB
{
    public class CircuitIterationRepository
    {
        private readonly IDynamoDbContextWrapper _dynamoDbContext;
        private readonly ITableWrapper _table;
        private readonly IUserInfoGetter _userInfoGetter;

        public CircuitIterationRepository(IDynamoDbContextWrapper dynamoDbContext, ITableWrapper table, IUserInfoGetter userInfoGetter)
        {
            _dynamoDbContext = dynamoDbContext;
            _table = table;
            _userInfoGetter = userInfoGetter;
        }

        public async Task<string> AddIterationAsync(string authorizationHeader, string circuitId)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var circuitIterationEntry = new CircuitIterationEntry
            {
                CircuitIterationPointer = new CircuitPointer
                {
                    UserId = userId,
                    CircuitId = circuitId
                },
                IterationId = Guid.NewGuid().ToString(),
                DateStarted = DateTime.UtcNow.ToString()
            };
            await _dynamoDbContext.SaveAsync(circuitIterationEntry);
            return circuitIterationEntry.IterationId;
        }

        public async Task<List<CircuitIterationListing>> GetIterationsAsync(string authorizationHeader, string circuitId)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var pointer = new CircuitPointer { UserId = userId, CircuitId = circuitId };
            var iterationEntries = await _dynamoDbContext.QueryWithEmptyBeginsWith<CircuitIterationEntry>(pointer);
            var circuitIterations = iterationEntries.Select(entry => new CircuitIterationListing
            {
                CircuitId = entry.CircuitIterationPointer.CircuitId,
                IterationId = entry.IterationId,
                DateStarted = entry.DateStarted,
                DateCompleted = entry.DateCompleted
            });
            return circuitIterations.ToList();
        }

        public async Task<CircuitIteration> GetIterationAsync(string authorizationHeader, string circuitId, string iterationId)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var pointer = new CircuitPointer { UserId = userId, CircuitId = circuitId };
            var iterationEntries = await _dynamoDbContext.QueryAsync<CircuitIterationEntry>(pointer, QueryOperator.Equal, new List<string> { iterationId });
            var iterationEntry = iterationEntries.Single();
            var circuitIterationPointerPropertyConverter = new CircuitIterationPointerConverter();
            var circuitIterationPointer = new CircuitIterationPointer
            {
                UserId = userId,
                CircuitId = circuitId,
                IterationId = iterationId
            };
            var pk = circuitIterationPointerPropertyConverter.ToEntry(circuitIterationPointer).AsString();
            var queryFilter = new QueryFilter(AttributeNames.SK, QueryOperator.BeginsWith, PropertyConverterConstants.ItemId);
            var search = _table.Query(pk, queryFilter);
            const string guidPattern = "[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}";
            const string itemId = PropertyConverterConstants.ItemId;
            const string occurrenceId = PropertyConverterConstants.OccurrenceId;
            const string setId = PropertyConverterConstants.SetId;
            const string equipmentInstanceId = PropertyConverterConstants.EquipmentInstanceId;
            const string recordedExercisePattern = $"^{itemId}#{guidPattern}#{occurrenceId}#{guidPattern}$";
            var recordedExerciseRegex = new Regex(recordedExercisePattern);
            const string exerciseSetPattern = $"^{itemId}#{guidPattern}#{occurrenceId}#{guidPattern}#{setId}#{guidPattern}$";
            var exerciseSetRegex = new Regex(exerciseSetPattern);
            const string equipmentInstancePattern = $"^{itemId}#{guidPattern}#{occurrenceId}#{guidPattern}#{setId}#{guidPattern}#{equipmentInstanceId}#{guidPattern}$";
            var equipmentInstanceRegex = new Regex(equipmentInstancePattern);
            var recordedExerciseEntries = new List<RecordedExerciseEntry>();
            var exerciseSetEntries = new List<ExerciseSetEntry>();
            var equipmentInstanceEntries = new List<EquipmentInstanceEntry>();
            do
            {
                var documentSet = await search.GetNextSetAsync();
                foreach (var document in documentSet)
                {
                    var sk = document[AttributeNames.SK].AsString();
                    if (recordedExerciseRegex.IsMatch(sk))
                    {
                        var recorderExerciseEntry = _dynamoDbContext.FromDocument<RecordedExerciseEntry>(document);
                        recordedExerciseEntries.Add(recorderExerciseEntry);
                    }
                    else if (exerciseSetRegex.IsMatch(sk))
                    {
                        var exerciseSetEntry = _dynamoDbContext.FromDocument<ExerciseSetEntry>(document);
                        exerciseSetEntries.Add(exerciseSetEntry);
                    }
                    else if (equipmentInstanceRegex.IsMatch(sk))
                    {
                        var equipmentInstanceEntry = _dynamoDbContext.FromDocument<EquipmentInstanceEntry>(document);
                        equipmentInstanceEntries.Add(equipmentInstanceEntry);
                    }
                }
            } while (!search.IsDone);

            var recordedExercises = new List<RecordedExercise>();
            foreach (var exercise in recordedExerciseEntries)
            {
                var exerciseSets = exerciseSetEntries.Where(
                    set => set.ExerciseSetPointer.ItemId == exercise.RecordedExercisePointer.ItemId &&
                    set.ExerciseSetPointer.OccurrenceId == exercise.RecordedExercisePointer.OccurrenceId)
                    .Select(set =>
                    {
                        var equipmentItems = equipmentInstanceEntries.Where(
                            equipment => equipment.EquipmentInstancePointer.ItemId == set.ExerciseSetPointer.ItemId &&
                            equipment.EquipmentInstancePointer.OccurrenceId == set.ExerciseSetPointer.OccurrenceId &&
                            equipment.EquipmentInstancePointer.SetId == set.ExerciseSetPointer.SetId).Select(equipment =>
                            new EquipmentInstance
                            {
                                EquipmentId = equipment.EquipmentId,
                                Count = equipment.Count,
                                Load = equipment.Load,
                                LoadUnit = (LoadUnit)equipment.LoadUnit
                            }).ToList();
                        return new ExerciseSet
                        {
                            EquipmentItems = equipmentItems,
                            Index = set.SetIndex,
                            RepetitionSize = set.RepetitionMeasurement
                        };
                    }).ToList();
                var recordedExercise = new RecordedExercise
                {
                    ExerciseId = exercise.ExerciseId,
                    ExerciseSets = exerciseSets,
                    DateCompleted = exercise.DateCompleted,
                };
                recordedExercises.Add(recordedExercise);
            }

            return new CircuitIteration
            {
                CircuitId = iterationEntry.CircuitIterationPointer.CircuitId,
                RecordedExercises = recordedExercises,
                DateStarted = iterationEntry.DateStarted,
                DateCompleted = iterationEntry.DateCompleted
            };
        }
    }
}
