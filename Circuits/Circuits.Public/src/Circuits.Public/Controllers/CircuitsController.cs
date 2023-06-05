using Circuits.Public.Controllers.Models;
using Circuits.Public.DynamoDB;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    public class CircuitsController : ControllerBase
    {
        private readonly CircuitsRepository _circuitsRepository;

        public CircuitsController(CircuitsRepository circuitsRepository)
        {
            _circuitsRepository = circuitsRepository;
        }

        [HttpPost("equipment")]
        public async Task<ActionResult<string>> AddEquipment([FromBody] AddEquipmentRequest request)
        {
            return await _circuitsRepository.AddEquipmentAsync(request);
        }
        
        [HttpPost("exercises")]
        public async Task<ActionResult<string>> AddExercise([FromBody] AddExerciseRequest request)
        {
            return await _circuitsRepository.AddExerciseAsync(request);
        }

        [HttpPost("circuits")]
        public async Task<ActionResult<string>> AddCircuit([FromBody] AddCircuitRequest request)
        {
            //var authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            //var accessToken = authorizationHeaderValue.Replace("Bearer ", string.Empty);

            // decode token

            // get user info from endpoint using access token

            // add circuit to dynamodb table
            return await _circuitsRepository.AddCircuitAsync(request.UserId, request.Name);
        }

        [HttpPost("circuits/item")]
        public async Task<ActionResult<string>> AddCircuitItem([FromBody] AddItemRequest request)
        {
            return await _circuitsRepository.AddItemAsync(request);
        }
    }
}
