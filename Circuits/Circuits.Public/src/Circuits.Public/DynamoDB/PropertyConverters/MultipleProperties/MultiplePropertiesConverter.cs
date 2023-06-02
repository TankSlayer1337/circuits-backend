using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters.MultipleProperties
{
    public abstract class MultiplePropertiesConverter<T> : IPropertyConverter where T : class
    {
        public abstract PropertyDefinition[] PropertyDefinitions { get; }
        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string assembledPattern = "^";
            for (var i = 0; i < PropertyDefinitions.Length; i++)
            {
                if (i != 0)
                    assembledPattern += "#";
                var propertyName = PropertyDefinitions[i].Name;
                var propertyPattern = PropertyDefinitions[i].RegExPattern;
                assembledPattern += $"{propertyName}#(?<g{i}>{propertyPattern})";
            }
            assembledPattern += "$";

            var regex = new Regex(assembledPattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var orderedValues = new List<string>();
                for (var i = 0; i < PropertyDefinitions.Length; i++)
                {
                    var value = match.Groups[$"g{i}"].Value;
                    orderedValues.Add(value);
                }

                return ToModel(orderedValues);
            }

            throw new ArgumentOutOfRangeException(nameof(entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var model = value as T ?? throw new ArgumentOutOfRangeException();
            var orderedValues = ToOrderedValues(model);
            var data = string.Empty;
            for (var i = 0; i < PropertyDefinitions.Length; i++)
            {
                if (i != 0)
                    data += "#";
                var propertyName = PropertyDefinitions[i].Name;
                var val = orderedValues[i];
                data += $"{propertyName}#{val}";
            }

            return data;
        }

        public abstract T ToModel(List<string> orderedValues);
        public abstract List<string> ToOrderedValues(T model);
    }
}
