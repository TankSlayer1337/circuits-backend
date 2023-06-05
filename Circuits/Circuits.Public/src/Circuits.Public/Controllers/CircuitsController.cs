using Circuits.Public.Controllers.Models.AddRequests;
using Circuits.Public.Controllers.Models.GetRequests;
using Circuits.Public.DynamoDB;
using Circuits.Public.PresentationModels.CircuitDefinitionModels;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    public class CircuitsController : ControllerBase
    {
        private readonly CircuitsRepository _circuitsRepository;
        private const string Equipment = "equipment";
        private const string Circuits = "circuits";

        public CircuitsController(CircuitsRepository circuitsRepository)
        {
            _circuitsRepository = circuitsRepository;
        }

        [HttpPost(Equipment)]
        public async Task<ActionResult<string>> AddEquipment([FromBody] AddEquipmentRequest request)
        {
            return await _circuitsRepository.AddEquipmentAsync(request);
        }

        [HttpGet(Equipment)]
        public async Task<ActionResult<List<Equipment>>> GetEquipment([FromBody] GetAllRequest request)
        {
            return await _circuitsRepository.GetEquipmentAsync(request.UserId);
        }
        
        [HttpPost("exercises")]
        public async Task<ActionResult<string>> AddExercise([FromBody] AddExerciseRequest request)
        {
            return await _circuitsRepository.AddExerciseAsync(request);
        }

        [HttpPost(Circuits)]
        public async Task<ActionResult<string>> AddCircuit([FromBody] AddCircuitRequest request)
        {
            //var authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            //var accessToken = authorizationHeaderValue.Replace("Bearer ", string.Empty);

            // decode token

            // get user info from endpoint using access token

            // add circuit to dynamodb table
            return await _circuitsRepository.AddCircuitAsync(request.UserId, request.Name);
        }

        [HttpGet(Circuits)]
        public async Task<ActionResult<List<ExerciseCircuit>>> GetCircuits([FromBody] GetAllRequest request)
        {
            return await _circuitsRepository.GetCircuitsAsync(request.UserId);
        }

        [HttpPost("circuits/items")]
        public async Task<ActionResult<string>> AddCircuitItem([FromBody] AddItemRequest request)
        {
            return await _circuitsRepository.AddItemAsync(request);
        }
    }
}
