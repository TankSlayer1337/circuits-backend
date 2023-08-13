using Circuits.Public.DynamoDB;
using Circuits.Public.Tests.Mockers;
using Circuits.Public.UserInfo;
using Microsoft.Extensions.DependencyInjection;

namespace Circuits.Public.Tests.CircuitIteration
{
    public class CircuitIterationRepositoryTestBase
    {
        protected readonly Faker _faker = new();
        protected readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();
        protected readonly HttpClientWrapperMocker _httpClientWrapperMocker = new();
        protected readonly EnvironmentVariableGetterMocker _environmentVariableGetterMocker = new();
        protected readonly TableQuerierMocker _tableQuerierMocker = new();

        public CircuitIterationRepository BuildCircuitIterationRepository()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_contextWrapperMocker.Mock.Object);
            services.AddSingleton(_httpClientWrapperMocker.Mock.Object);
            services.AddSingleton(_environmentVariableGetterMocker.Mock.Object);
            services.AddSingleton(_tableQuerierMocker.Mock.Object);
            services.AddTransient<CircuitIterationRepository>();
            services.AddTransient<IUserInfoGetter, UserInfoGetter>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<CircuitIterationRepository>();
        }
    }
}
