namespace Circuits.Public.DynamoDB.DynamoDBModels
{
    public class CircuitIterationPointer
    {
        public string CircuitId { get; init; } = string.Empty;
        public int IterationIndex { get; init; } = 0;
    }
}
