using Circuits.Public.Controllers;
using Circuits.Public.DynamoDB;
using Circuits.Public.Tests.Mockers;
using Microsoft.Extensions.DependencyInjection;

namespace Circuits.Public.Tests.AddingUserData
{
    internal class TestHelper
    {
        public static CircuitsController BuildCircuitsController(DynamoDbContextWrapperMocker contextWrapperMocker)
        {
            var services = new ServiceCollection();
            services.AddSingleton(contextWrapperMocker.Mock.Object);
            services.AddTransient<CircuitsRepository>();
            services.AddTransient<CircuitsController>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<CircuitsController>();
        }
    }
}
