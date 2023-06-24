using Circuits.Public.DynamoDB;
using Circuits.Public.Tests.Mockers;
using Circuits.Public.UserInfo;
using Microsoft.Extensions.DependencyInjection;

namespace Circuits.Public.Tests.CircuitDefinition
{
    public class CircuitsRepositoryTestBase
    {
        protected readonly Faker _faker = new();
        protected readonly DynamoDbContextWrapperMocker _contextWrapperMocker = new();
        protected readonly HttpClientWrapperMocker _httpClientWrapperMocker = new();
        protected readonly EnvironmentVariableGetterMocker _environmentVariableGetterMocker = new();

        public CircuitsRepository BuildCircuitsRepository()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_contextWrapperMocker.Mock.Object);
            services.AddSingleton(_httpClientWrapperMocker.Mock.Object);
            services.AddSingleton(_environmentVariableGetterMocker.Mock.Object);
            services.AddTransient<CircuitsRepository>();
            services.AddTransient<IUserInfoGetter, UserInfoGetter>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<CircuitsRepository>();
        }
    }
}
