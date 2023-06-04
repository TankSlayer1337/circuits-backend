using Circuits.Public.Controllers.Models;
using Circuits.Public.DynamoDB;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    public class CircuitsController : ControllerBase
    {
        [HttpPost("exercises")]
        public async Task<ActionResult<string>> AddExercise([FromServices] CircuitsRepository circuitsRepository, [FromBody] AddExerciseRequest request)
        {
            return await circuitsRepository.AddExerciseAsync(request);
        }

        [HttpPost("circuits")]
        public async Task<ActionResult<string>> AddCircuit([FromServices] CircuitsRepository circuitsRepository, [FromBody] AddCircuitRequest request)
        {
            //var authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            //var accessToken = authorizationHeaderValue.Replace("Bearer ", string.Empty);

            // decode token

            // get user info from endpoint using access token

            // add circuit to dynamodb table
            return await circuitsRepository.AddCircuitAsync(request.UserId, request.Name);
        }
    }
}
