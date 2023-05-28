using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.DynamoDBModels;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public class CircuitIterationPointerConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{PropertyConverterConstants.CircuitId}#(?<guid>{PropertyConverterConstants.GuidPattern})#IterationIndex#(?<index>\\d+)$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var circuitId = match.Groups["guid"].Value;
                var index = match.Groups["index"].Value;

                return new CircuitIterationPointer
                {
                    CircuitId = circuitId,
                    IterationIndex = int.Parse(index)
                };
            }

            throw new NotImplementedException();
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var pointer = value as CircuitIterationPointer ?? throw new ArgumentOutOfRangeException();
            string data = $"CircuitId#{pointer.CircuitId}#IterationIndex#{pointer.IterationIndex}";

            return data;
        }
    }
}
