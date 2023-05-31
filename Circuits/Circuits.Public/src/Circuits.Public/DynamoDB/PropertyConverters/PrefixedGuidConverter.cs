﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    public abstract class PrefixedGuidConverter : IPropertyConverter
    {
        public abstract string Prefix { get; }

        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{Prefix}#(?<id>{PropertyConverterConstants.GuidPattern})$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                return match.Groups["id"].Value;
            }

            throw new ArgumentOutOfRangeException(nameof (entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var data = (string)value;
            if (Guid.TryParse(data, out var _)) {
                return $"{Prefix}#{data}";
            }

            throw new ArgumentOutOfRangeException(nameof (value));
        }
    }

    public class UserIdConverter : PrefixedGuidConverter
    {
        public override string Prefix => PropertyConverterConstants.UserId;
    }

    public class CircuitIdConverter : PrefixedGuidConverter
    {
        public override string Prefix => PropertyConverterConstants.CircuitId;
    }

    public class ExerciseIdConverter : PrefixedGuidConverter
    {
        public override string Prefix => AttributeNames.ExerciseId;
    }

    public class EquipmentIdConverter : PrefixedGuidConverter
    {
        public override string Prefix => AttributeNames.EquipmentId;
    }

    public class IterationIdConverter : PrefixedGuidConverter
    {
        public override string Prefix => PropertyConverterConstants.IterationId;
    }
}