using Circuits.Public.DynamoDB.Models.ExerciseCircuit;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class CircuitItemPointerConverter : MultiplePropertiesConverter<CircuitItemPointer>
    {
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public override PropertyDefinition[] PropertyDefinitions => new PropertyDefinition[]
        {
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.CircuitId,
                RegExPattern = GuidPattern
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.ItemId,
                RegExPattern = GuidPattern
            }
        };

        public override CircuitItemPointer ToModel(List<string> orderedValues)
        {
            return new CircuitItemPointer
            {
                CircuitId = orderedValues[0],
                ItemId = orderedValues[1]
            };
        }

        public override List<string> ToOrderedValues(CircuitItemPointer model)
        {
            return new List<string> { model.CircuitId, model.ItemId };
        }
    }
}
