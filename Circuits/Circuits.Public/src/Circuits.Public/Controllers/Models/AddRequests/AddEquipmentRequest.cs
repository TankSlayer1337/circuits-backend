namespace Circuits.Public.Controllers.Models.AddRequests
{
    public class AddEquipmentRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool CanBeUsedInMultiples { get; init; }
    }
}
