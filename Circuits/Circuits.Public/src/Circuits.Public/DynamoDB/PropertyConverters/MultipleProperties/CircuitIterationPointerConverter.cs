using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class CircuitIterationPointerConverter : MultiplePropertiesConverter<CircuitIterationPointer>
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
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.IterationId,
                RegExPattern = GuidPattern
            }
        };

        public override CircuitIterationPointer ToModel(List<string> orderedValues)
        {
            return new CircuitIterationPointer
            {
                UserId = orderedValues[0],
                CircuitId = orderedValues[1],
                IterationId = orderedValues[2]
            };
        }

        public override List<string> ToOrderedValues(CircuitIterationPointer model)
        {
            return new List<string> { model.UserId, model.CircuitId, model.IterationId };
        }
    }
}
