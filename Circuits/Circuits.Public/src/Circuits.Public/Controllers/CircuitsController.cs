using Circuits.Public.DynamoDB;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    public class CircuitsController : ControllerBase
    {
        // the userId parameter will be removed when authorization has been setup.
        [HttpPost("circuits/{userId}/{name}")]
        public async Task<ActionResult<string>> AddCircuit([FromServices] CircuitsRepository circuitsRepository, string userId, string name)
        {
            //var authorizationHeaderValue = Request.Headers["Authorization"].ToString();
            //var accessToken = authorizationHeaderValue.Replace("Bearer ", string.Empty);

            // decode token

            // get user info from endpoint using access token

            // add circuit to dynamodb table
            var circuitId = await circuitsRepository.AddCircuitAsync(userId, name);

            return circuitId;
        }
    }
}
