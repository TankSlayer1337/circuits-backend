using Circuits.Public.DynamoDB;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    public class IterationController : ControllerBase
    {
        private readonly CircuitIterationRepository _circuitIterationRepository;

        public IterationController(CircuitIterationRepository circuitIterationRepository)
        {
            _circuitIterationRepository = circuitIterationRepository;
        }

        [HttpPost("{circuitId}")]
        public async Task<ActionResult<string>> AddIteration(string circuitId)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.AddIterationAsync(circuitId, authorizationHeader);
        }
    }
}
