namespace Circuits.Public.Controllers.Models.GetRequests
{
    public class GetItemsRequest : GetAllRequest
    {
        public string CircuitId { get; init; } = string.Empty;
    }
}
