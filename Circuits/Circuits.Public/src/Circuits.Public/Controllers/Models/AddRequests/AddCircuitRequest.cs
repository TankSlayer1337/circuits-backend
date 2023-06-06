namespace Circuits.Public.Controllers.Models.AddRequests
{
    public class AddCircuitRequest
    {
        // UserId will be removed when access tokens are used.
        public string UserId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}
