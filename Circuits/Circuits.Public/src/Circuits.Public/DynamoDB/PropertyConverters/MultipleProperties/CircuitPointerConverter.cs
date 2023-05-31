using Circuits.Public.DynamoDB.Models.CircuitIteration;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class CircuitPointerConverter : MultiplePropertiesConverter<CircuitPointer>
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

        public override CircuitPointer ToModel(List<string> orderedValues)
        {
            return new CircuitPointer
            {
                UserId = orderedValues[0],
                CircuitId = orderedValues[1]
            };
        }

        public override List<string> ToOrderedValues(CircuitPointer model)
        {
            return new List<string> { model.UserId, model.CircuitId };
        }
    }
}
