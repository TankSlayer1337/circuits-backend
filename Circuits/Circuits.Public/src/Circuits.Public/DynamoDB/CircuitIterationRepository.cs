using Circuits.Public.DynamoDB.Models.CircuitIteration;
using Circuits.Public.UserInfo;

namespace Circuits.Public.DynamoDB
{
    public class CircuitIterationRepository
    {
        private readonly IDynamoDbContextWrapper _dynamoDbContext;
        private readonly IUserInfoGetter _userInfoGetter;

        public CircuitIterationRepository(IDynamoDbContextWrapper dynamoDbContext, IUserInfoGetter userInfoGetter)
        {
            _dynamoDbContext = dynamoDbContext;
            _userInfoGetter = userInfoGetter;
        }

        public async Task<string> AddIterationAsync(string authorizationHeader, string circuitId)
        {
            var userId = await _userInfoGetter.GetUserIdAsync(authorizationHeader);
            var circuitIterationEntry = new CircuitIterationEntry
            {
                CircuitIterationPointer = new CircuitPointer
                {
                    UserId = userId,
                    CircuitId = circuitId
                },
                IterationId = Guid.NewGuid().ToString(),
                DateStarted = DateTime.UtcNow.ToString()
            };
            await _dynamoDbContext.SaveAsync(circuitIterationEntry);
            return circuitIterationEntry.IterationId;
        }
    }
}
