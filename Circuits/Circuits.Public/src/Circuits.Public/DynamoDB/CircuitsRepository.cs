using Circuits.Public.Controllers.Models;
using Circuits.Public.DynamoDB.Models.ExerciseCircuit;

namespace Circuits.Public.DynamoDB
{
    public class CircuitsRepository
    {
        private readonly DynamoDbContextWrapper _dynamoDbContext;

        public CircuitsRepository(DynamoDbContextWrapper dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<string> AddCircuitAsync(string userId, string name)
        {
            var circuitEntry = new ExerciseCircuitEntry
            {
                UserId = userId,
                CircuitId = Guid.NewGuid().ToString(),
                Name = name
            };
            await _dynamoDbContext.SaveAsync(circuitEntry);
            return circuitEntry.CircuitId;
        }

        public async Task<string> AddExerciseAsync(AddExerciseRequest request)
        {
            var exerciseEntry = new ExerciseEntry
            {
                UserId = request.UserId,
                ExerciseId = Guid.NewGuid().ToString(),
                Name = request.Name,
                RepetitionType = request.RepetitionType,
                DefaultEquipmentId = request.DefaultEquipmentId
            };
            await _dynamoDbContext.SaveAsync(exerciseEntry);
            return exerciseEntry.ExerciseId;
        }
    }
}
