using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using Circuits.Public.DynamoDB.PropertyConverters;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB
{
    public class TableQuerier : ITableQuerier
    {
        private readonly ITableWrapper _table;
        private readonly IDynamoDbContextWrapper _dynamoDbContext;
        const string GuidPattern = "[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}";
        const string ItemId = PropertyConverterConstants.ItemId;
        const string OccurrenceId = PropertyConverterConstants.OccurrenceId;
        const string SetId = PropertyConverterConstants.SetId;
        const string EquipmentInstanceId = PropertyConverterConstants.EquipmentInstanceId;
        const string RecordedExercisePattern = $"^{ItemId}#{GuidPattern}#{OccurrenceId}#{GuidPattern}$";
        private readonly Regex RecordedExerciseRegex = new Regex(RecordedExercisePattern);
        const string ExerciseSetPattern = $"^{ItemId}#{GuidPattern}#{OccurrenceId}#{GuidPattern}#{SetId}#{GuidPattern}$";
        private readonly Regex ExerciseSetRegex = new Regex(ExerciseSetPattern);
        const string EquipmentInstancePattern = $"^{ItemId}#{GuidPattern}#{OccurrenceId}#{GuidPattern}#{SetId}#{GuidPattern}#{EquipmentInstanceId}#{GuidPattern}$";
        private readonly Regex EquipmentInstanceRegex = new Regex(EquipmentInstancePattern);

        public TableQuerier(ITableWrapper table, IDynamoDbContextWrapper dynamoDbContext)
        {
            _table = table;
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<IterationQueryResult> RunIterationQueryAsync(CircuitIterationPointer iterationPointer)
        {
            var circuitIterationPointerPropertyConverter = new CircuitIterationPointerConverter();
            var pk = circuitIterationPointerPropertyConverter.ToEntry(iterationPointer).AsString();
            var queryFilter = new QueryFilter(AttributeNames.SK, QueryOperator.BeginsWith, PropertyConverterConstants.ItemId);
            var search = _table.Query(pk, queryFilter);
            var recordedExerciseEntries = new List<RecordedExerciseEntry>();
            var exerciseSetEntries = new List<ExerciseSetEntry>();
            var equipmentInstanceEntries = new List<EquipmentInstanceEntry>();
            do
            {
                var documentSet = await search.GetNextSetAsync();
                foreach (var document in documentSet)
                {
                    var sk = document[AttributeNames.SK].AsString();
                    if (RecordedExerciseRegex.IsMatch(sk))
                    {
                        var recorderExerciseEntry = _dynamoDbContext.FromDocument<RecordedExerciseEntry>(document);
                        recordedExerciseEntries.Add(recorderExerciseEntry);
                    }
                    else if (ExerciseSetRegex.IsMatch(sk))
                    {
                        var exerciseSetEntry = _dynamoDbContext.FromDocument<ExerciseSetEntry>(document);
                        exerciseSetEntries.Add(exerciseSetEntry);
                    }
                    else if (EquipmentInstanceRegex.IsMatch(sk))
                    {
                        var equipmentInstanceEntry = _dynamoDbContext.FromDocument<EquipmentInstanceEntry>(document);
                        equipmentInstanceEntries.Add(equipmentInstanceEntry);
                    }
                }
            } while (!search.IsDone);

            return new IterationQueryResult
            {
                ExerciseEntries = recordedExerciseEntries,
                ExerciseSets = exerciseSetEntries,
                EquipmentInstances = equipmentInstanceEntries
            };
        }
    }
}
