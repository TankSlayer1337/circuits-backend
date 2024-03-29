﻿using Circuits.Public.Controllers.Models.AddRequests;
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
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.AddEquipmentAsync(authorizationHeader, request);
        }

        [HttpGet(Equipment)]
        public async Task<ActionResult<List<Equipment>>> GetEquipment()
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.GetEquipmentAsync(authorizationHeader);
        }
        
        [HttpPost(Exercises)]
        public async Task<ActionResult<string>> AddExercise([FromBody] AddExerciseRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.AddExerciseAsync(authorizationHeader, request);
        }

        [HttpGet(Exercises)]
        public async Task<ActionResult<List<Exercise>>> GetExercises()
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.GetExercisesAsync(authorizationHeader);
        }

        [HttpPost(Circuits)]
        public async Task<ActionResult<string>> AddCircuit([FromBody] AddCircuitRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.AddCircuitAsync(authorizationHeader, request.Name);
        }

        [HttpGet(Circuits)]
        public async Task<ActionResult<List<Circuit>>> GetCircuits()
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.GetCircuitsAsync(authorizationHeader);
        }

        [HttpPost(Items)]
        public async Task<ActionResult<string>> AddCircuitItem([FromBody] AddItemRequest request)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.AddItemAsync(authorizationHeader, request);
        }

        [HttpGet(Items+"/{circuitId}")]
        public async Task<ActionResult<List<Item>>> GetCircuitItems(string circuitId)
        {
            var authorizationHeader = Utils.GetAuthorizationHeader(Request);
            return await _circuitsRepository.GetItemsAsyncWithAuthorizationHeader(authorizationHeader, circuitId);
        }
    }
}
