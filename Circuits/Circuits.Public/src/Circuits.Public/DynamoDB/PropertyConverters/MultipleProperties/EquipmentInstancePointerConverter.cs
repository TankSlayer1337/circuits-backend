using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public class EquipmentInstancePointerConverter : MultiplePropertiesConverter<EquipmentInstancePointer>
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
            },
            new PropertyDefinition
            {
                Name = PropertyConverterConstants.EquipmentInstanceId,
                RegExPattern = GuidPattern
            }
        };

        public override EquipmentInstancePointer ToModel(List<string> orderedValues)
        {
            return new EquipmentInstancePointer
            {
                ItemId = orderedValues[0],
                OccurrenceId = orderedValues[1],
                SetId = orderedValues[2],
                EquipmentInstanceId = orderedValues[3]
            };
        }

        public override List<string> ToOrderedValues(EquipmentInstancePointer model)
        {
            return new List<string> { model.ItemId, model.OccurrenceId, model.SetId, model.EquipmentInstanceId };
        }
    }
}
