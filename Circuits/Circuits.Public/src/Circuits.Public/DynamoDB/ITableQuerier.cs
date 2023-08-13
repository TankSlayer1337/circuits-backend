using Circuits.Public.DynamoDB.Models.CircuitIteration.IterationModels;

namespace Circuits.Public.DynamoDB
{
    public interface ITableQuerier
    {
        Task<IterationQueryResult> RunIterationQueryAsync(CircuitIterationPointer iterationPointer);
    }
}