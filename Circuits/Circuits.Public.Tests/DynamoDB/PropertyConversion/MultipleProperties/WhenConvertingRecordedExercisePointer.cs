using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties;

namespace Circuits.Public.Tests.DynamoDB.PropertyConversion.MultipleProperties
{
    public class WhenConvertingRecordedExercisePointer : MultiplePropertiesConversionTest<RecordedExercisePointer, RecordedExercisePointerConverter>
    {
        protected override (RecordedExercisePointer propertyInstance, string entryValue) CreatePropertyAndStringRepresentation()
        {
            var propertyInstance = new RecordedExercisePointer
            {
                ItemId = TestData.CreateRandomGuidValue(),
                OccurrenceId = TestData.CreateRandomGuidValue()
            };
            var entryValue = $"ItemId#{propertyInstance.ItemId}#OccurrenceId#{propertyInstance.OccurrenceId}";
            return (propertyInstance, entryValue);
        }
    }
}
