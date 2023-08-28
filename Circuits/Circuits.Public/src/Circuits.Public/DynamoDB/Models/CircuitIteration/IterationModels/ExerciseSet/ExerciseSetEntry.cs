using Amazon.DynamoDBv2.DataModel;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;

namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet
{
    public class ExerciseSetEntry
    {
        [DynamoDBHashKey(AttributeNames.PK, typeof(CircuitIterationPointerConverter))]
        public CircuitIterationPointer CircuitIterationPointer { get; init; }

        [DynamoDBRangeKey(AttributeNames.SK, typeof(ExerciseSetPointerConverter))]
        public ExerciseSetPointer ExerciseSetPointer { get; init; }

        [DynamoDBProperty(AttributeNames.SetIndex)]
        public int SetIndex { get; init; }

        [DynamoDBProperty(AttributeNames.RepetitionType)]
        public RepetitionType RepetitionType { get; init; }

        [DynamoDBProperty(AttributeNames.RepetitionMeasurement)]
        public string RepetitionMeasurement { get; init; } = string.Empty;
    }
}
