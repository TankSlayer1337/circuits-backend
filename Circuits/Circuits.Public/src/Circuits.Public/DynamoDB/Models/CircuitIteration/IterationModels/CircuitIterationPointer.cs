namespace Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels
{
    public class CircuitIterationPointer : CircuitPointer
    {
        public string IterationId { get; init; } = string.Empty;

        public CircuitIterationPointer Clone()
        {
            return new CircuitIterationPointer
            {
                CircuitId = CircuitId,
                UserId = UserId,
                IterationId = IterationId
            };
        }
    }
}
