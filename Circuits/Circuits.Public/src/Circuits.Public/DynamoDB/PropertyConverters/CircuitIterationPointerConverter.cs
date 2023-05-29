﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;
using System.Text.RegularExpressions;

namespace Circuits.Public.DynamoDB.PropertyConverters
{
    // TODO: refactor out the common parts between this and CircuitPointerConverter.
    public class CircuitIterationPointerConverter : IPropertyConverter
    {
        private const string UserId = PropertyConverterConstants.UserId;
        private const string CircuitId = PropertyConverterConstants.CircuitId;
        private const string IterationId = PropertyConverterConstants.IterationId;
        private const string GuidPattern = PropertyConverterConstants.GuidPattern;

        public object FromEntry(DynamoDBEntry entry)
        {
            var data = (string)entry;
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentOutOfRangeException(nameof(entry));
            }

            string pattern = $"^{UserId}#(?<user-id>{GuidPattern})#{CircuitId}#(?<circuit-id>{GuidPattern})#{IterationId}#(?<iteration-id>{GuidPattern})$";
            var regex = new Regex(pattern);
            var match = regex.Match(data);

            if (match.Success)
            {
                var userId = match.Groups["user-id"].Value;
                var circuitId = match.Groups["circuit-id"].Value;
                var iterationId = match.Groups["iteration-id"].Value;

                return new CircuitIterationPointer
                {
                    UserId = userId,
                    CircuitId = circuitId,
                    IterationId = iterationId,
                };
            }

            throw new ArgumentOutOfRangeException(nameof(entry));
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var pointer = value as CircuitIterationPointer ?? throw new ArgumentOutOfRangeException();
            string data = $"{UserId}#{pointer.UserId}#{CircuitId}#{pointer.CircuitId}#{IterationId}#{pointer.IterationId}";

            return data;
        }
    }
}
