using Circuits.Public.DynamoDB;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Microsoft.AspNetCore.Mvc;

namespace Circuits.Public.Controllers
{
    [ApiController]
    public class IterationController : ControllerBase
    {
        private readonly CircuitIterationRepository _circuitIterationRepository;

        public IterationController(CircuitIterationRepository circuitIterationRepository)
        {
            _circuitIterationRepository = circuitIterationRepository;
        }

        [HttpPost("iteration")]
        public async Task<ActionResult<string>> AddIteration([FromQuery] string circuitId)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.AddIterationAsync(authorizationHeader, circuitId);
        }

        [HttpGet("iterations")]
        public async Task<ActionResult<List<CircuitIterationListing>>> GetIterations([FromQuery] string circuitId)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.GetIterationsAsync(authorizationHeader, circuitId);
        }

        [HttpGet("iteration")]
        public async Task<ActionResult<CircuitIteration>> GetIteration([FromQuery] string circuitId, [FromQuery] string iterationId)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.GetIterationAsync(authorizationHeader, circuitId, iterationId);
        }
    }
}
