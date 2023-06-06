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
        private const string Exercises = "exercises";
        private const string Items = "items";

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
        
        [HttpPost(Exercises)]
        public async Task<ActionResult<string>> AddExercise([FromBody] AddExerciseRequest request)
        {
            return await _circuitsRepository.AddExerciseAsync(request);
        }

        [HttpGet(Exercises)]
        public async Task<ActionResult<List<Exercise>>> GetExercises([FromBody] GetAllRequest request)
        {
            return await _circuitsRepository.GetExercisesAsync(request.UserId);
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
        public async Task<ActionResult<List<Circuit>>> GetCircuits([FromBody] GetAllRequest request)
        {
            return await _circuitsRepository.GetCircuitsAsync(request.UserId);
        }

        [HttpPost(Items)]
        public async Task<ActionResult<string>> AddCircuitItem([FromBody] AddItemRequest request)
        {
            return await _circuitsRepository.AddItemAsync(request);
        }

        [HttpGet(Items)]
        public async Task<ActionResult<List<Item>>> GetCircuitItems([FromBody] GetItemsRequest request)
        {
            return await _circuitsRepository.GetItemsAsync(request.UserId, request.CircuitId);
        }
    }
}
