using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public class EnumPropertyConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            var enumValue = (int)entry;
            return enumValue;
        }

        public DynamoDBEntry ToEntry(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var entry = (int)value;
            return entry;
        }
    }
}
