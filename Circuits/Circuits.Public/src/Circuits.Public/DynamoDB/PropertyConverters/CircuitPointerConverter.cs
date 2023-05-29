using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitIteration;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public class CircuitPointerConverter : IPropertyConverter
    {
        private const string UserId = PropertyConverterConstants.UserId;
        private const string CircuitId = PropertyConverterConstants.CircuitId;
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{UserId}#(?<user-id>{GuidPattern})#{CircuitId}#(?<circuit-id>{GuidPattern})$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var userId = match.Groups["user-id"].Value;
                var circuitId = match.Groups["circuit-id"].Value;

                return new CircuitPointer
                {
                    UserId = userId,
                    CircuitId = circuitId
                };
            }

            throw new ArgumentOutOfRangeException(nameof(entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var pointer = value as CircuitPointer ?? throw new ArgumentOutOfRangeException();
            string data = $"{UserId}#{pointer.UserId}#{CircuitId}#{pointer.CircuitId}";

            return data;
        }
    }
}
