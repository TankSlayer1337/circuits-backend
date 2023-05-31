using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class ExerciseSetPointerConverter : MultiplePropertiesConverter<ExerciseSetPointer>
    {
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;
        public override PropertyDefinition[] PropertyDefinitions => new PropertyDefinition[]
        {
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.ItemId,
                RegExPattern = GuidPattern
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.OccurrenceId,
                RegExPattern = GuidPattern
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.SetId,
                RegExPattern = GuidPattern
            }
        };

        public override ExerciseSetPointer ToModel(List<string> orderedValues)
        {
            return new ExerciseSetPointer
            {
                ItemId = orderedValues[0],
                OccurrenceId = orderedValues[1],
                SetId = orderedValues[2]
            };
        }

        public override List<string> ToOrderedValues(ExerciseSetPointer model)
        {
            return new List<string> { model.ItemId, model.OccurrenceId, model.SetId };
        }
    }
}
