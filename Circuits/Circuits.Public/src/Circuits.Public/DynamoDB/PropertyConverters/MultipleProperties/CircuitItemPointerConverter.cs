using Circuits.Public.DynamoDB.Models.CircuitDefinition;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class CircuitItemPointerConverter : MultiplePropertiesConverter<CircuitItemPointer>
    {
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public override PropertyDefinition[] PropertyDefinitions => new PropertyDefinition[]
        {
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.UserId,
                RegExPattern = GuidPattern
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.CircuitId,
                RegExPattern = GuidPattern
            }
        };

        public override CircuitItemPointer ToModel(List<string> orderedValues)
        {
            return new CircuitItemPointer
            {
                UserId = orderedValues[0],
                CircuitId = orderedValues[1]
            };
        }

        public override List<string> ToOrderedValues(CircuitItemPointer model)
        {
            return new List<string> { model.UserId, model.CircuitId };
        }
    }
}
