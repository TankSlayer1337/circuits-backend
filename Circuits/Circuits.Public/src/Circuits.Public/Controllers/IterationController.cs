﻿using Circuits.Public.Controllers.Models.IterationModels;
using Circuits.Public.DynamoDB;
using Circuits.Public.PresentationModels.CircuitRecordingModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<List<CircuitIterationListing>>> GetIterationListings([FromQuery] string circuitId)
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

        // TODO: remove this as it won't be needed when iteration is filled with empty items.
        [HttpPost("iteration/exercise")]
        public async Task<ActionResult<string>> AddExercise([FromBody] AddRecordedExerciseRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.AddRecordedExercise(authorizationHeader, request);
        }

        [HttpPost("iteration/set")]
        public async Task<ActionResult<string>> AddSet([FromBody] AddExerciseSetRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.AddExerciseSet(authorizationHeader, request);
        }

        [HttpPost("iteration/equipment")]
        public async Task<ActionResult<string>> AddEquipment([FromBody] AddEquipmentInstanceRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitIterationRepository.AddEquipmentInstance(authorizationHeader, request);
        }
    }
}
