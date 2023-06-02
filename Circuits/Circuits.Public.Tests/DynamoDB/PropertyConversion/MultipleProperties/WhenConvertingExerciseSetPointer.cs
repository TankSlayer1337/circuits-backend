using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingExerciseSetPointer : MultiplePropertiesConversionTest<ExerciseSetPointer, ExerciseSetPointerConverter>
    {
        protected override (ExerciseSetPointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var itemId = TestData.CreateRandomGuidValue();
            var occurrenceId = TestData.CreateRandomGuidValue();
            var setId = TestData.CreateRandomGuidValue();
            var propertyInstance = new ExerciseSetPointer
            {
                ItemId = itemId,
                OccurrenceId = occurrenceId,
                SetId = setId
            };
            var entryValue = $"ItemId#{itemId}#OccurrenceId#{occurrenceId}#SetId#{setId}";
            return (propertyInstance,  entryValue);
        }
    }
}
