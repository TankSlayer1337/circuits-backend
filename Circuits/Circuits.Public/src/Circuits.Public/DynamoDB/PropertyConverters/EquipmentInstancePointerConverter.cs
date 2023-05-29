using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.EquipmentInstance;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public class EquipmentInstancePointerConverter : IPropertyConverter
    {
        private const string ItemId = PropertyConverterConstants.ItemId;
        private const string SetId = PropertyConverterConstants.SetId;
        private const string EquipmentInstanceId = PropertyConverterConstants.EquipmentInstanceId;
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{ItemId}#(?<item-id>{GuidPattern})#{SetId}#(?<set-id>{GuidPattern})#{EquipmentInstanceId}#(?<instance-id>{GuidPattern})$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var itemId = match.Groups["item-id"].Value;
                var setId = match.Groups["set-id"].Value;
                var instanceId = match.Groups["instance-id"].Value;

                return new EquipmentInstancePointer
                {
                    ItemId = itemId,
                    SetId = setId,
                    EquipmentInstanceId = instanceId,
                };
            }

            throw new ArgumentOutOfRangeException(nameof(entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var pointer = value as EquipmentInstancePointer ?? throw new ArgumentOutOfRangeException();
            string data = $"{ItemId}#{pointer.ItemId}#{SetId}#{pointer.SetId}#{EquipmentInstanceId}#{pointer.EquipmentInstanceId}";

            return data;
        }
    }
}
