using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Extensions;
using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Circuits.Public.UserInfo;

namespace Circuits.Public.DynamoDB
{
    public class CircuitIterationRepository
    {
        private readonly IDynamoDbContextWrapper _dynamoDbContext;
        private readonly IUserInfoGetter _userInfoGetter;
        private readonly ITableQuerier _tableQuerier;

        public CircuitIterationRepository(IDynamoDbContextWrapper dynamoDbContext, IUserInfoGetter userInfoGetter, ITableQuerier tableQuerier)
        {
            _dynamoDbContext = dynamoDbContext;
            _userInfoGetter = userInfoGetter;
            _tableQuerier = tableQuerier;
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
            var circuitIterationPointer = new CircuitIterationPointer
            {
                UserId = userId,
                CircuitId = circuitId,
                IterationId = iterationId
            };
            var iterationQueryResult = await _tableQuerier.RunIterationQueryAsync(circuitIterationPointer);
            var recordedExercises = ExtractRecordedExercises(iterationQueryResult);
            return new CircuitIteration
            {
                CircuitId = iterationEntry.CircuitIterationPointer.CircuitId,
                RecordedExercises = recordedExercises,
                DateStarted = iterationEntry.DateStarted,
                DateCompleted = iterationEntry.DateCompleted
            };
        }

        private static List<RecordedExercise> ExtractRecordedExercises(IterationQueryResult iterationQueryResult)
        {
            var recordedExercises = new List<RecordedExercise>();
            foreach (var exercise in iterationQueryResult.ExerciseEntries)
            {
                var exerciseSets = iterationQueryResult.ExerciseSets.Where(
                    set => set.ExerciseSetPointer.ItemId == exercise.RecordedExercisePointer.ItemId &&
                    set.ExerciseSetPointer.OccurrenceId == exercise.RecordedExercisePointer.OccurrenceId)
                    .Select(set =>
                    {
                        var equipmentItems = iterationQueryResult.EquipmentInstances.Where(
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
            return recordedExercises;
        }
    }
}
