﻿using Amazon.DynamoDBv2.DataModel;

namespace Circuits.Public.DynamoDB.DynamoDBModels
{
    public class EquipmentEntry : TableEntry
    {
        [DynamoDBHashKey]
        public string UserId { get; init; } = string.Empty;

        [DynamoDBRangeKey]
        public string EquipmentId { get; init; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string EquipmentName { get; init; } = string.Empty;
    }
}
