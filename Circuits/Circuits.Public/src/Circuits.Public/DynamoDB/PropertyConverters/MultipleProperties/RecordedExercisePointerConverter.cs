using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class RecordedExercisePointerConverter : MultiplePropertiesConverter<RecordedExercisePointer>
    {
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public override PropertyDefinition[] PropertyDefinitions => new PropertyDefinition[]
        {
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.ItemId,
                RegExPattern = GuidPattern,
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.OccurrenceId,
                RegExPattern = GuidPattern
            }
        };

        public override RecordedExercisePointer ToModel(List<string> orderedValues)
        {
            return new RecordedExercisePointer
            {
                ItemId = orderedValues[0],
                OccurrenceId = orderedValues[1]
            };
        }

        public override List<string> ToOrderedValues(RecordedExercisePointer model)
        {
            return new List<string> { model.ItemId, model.OccurrenceId };
        }
    }
}
