using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels.ExerciseSet;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public class ExerciseSetPointerConverter : IPropertyConverter
    {
        private const string ItemId = PropertyConverterConstants.ItemId;
        private const string SetId = PropertyConverterConstants.SetId;
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{ItemId}#(?<item-id>{GuidPattern})#{SetId}#(?<set-id>{GuidPattern})$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var itemId = match.Groups["item-id"].Value;
                var setId = match.Groups["set-id"].Value;

                return new ExerciseSetPointer
                {
                    ItemId = itemId,
                    SetId = setId
                };
            }

            throw new ArgumentOutOfRangeException(nameof(entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var pointer = value as ExerciseSetPointer ?? throw new ArgumentOutOfRangeException();
            string data = $"{ItemId}#{pointer.ItemId}#{SetId}#{pointer.SetId}";

            return data;
        }
    }
}
