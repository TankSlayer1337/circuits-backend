using Circuits.Public.Tests.Mockers;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace Circuits.Public.Tests.Utils
{
    internal static class UserInfoEndpointSimulator
    {
        private static readonly Faker _faker = new();

        public static (string userId, string authorizationHeader) SimulateUserInfoEndpoint(HttpClientWrapperMocker httpClientWrapperMocker, EnvironmentVariableGetterMocker environmentVariableGetterMocker)
        {
            var userInfoEndpointUrl = _faker.Internet.Url();
            environmentVariableGetterMocker.SimulateGet("USERINFO_ENDPOINT_URL", userInfoEndpointUrl);
            var authorizationHeader = $"Bearer {_faker.Random.AlphaNumeric(100)}";
            var userInfoRequest = BuildExpectedHttpRequestMessage(userInfoEndpointUrl, authorizationHeader);
            var userId = _faker.Random.AlphaNumeric(10);
            var userInfoResponse = BuildUserInfoEndpointResponse(userId);
            httpClientWrapperMocker.SimulateSendAsync(userInfoRequest, userInfoResponse);
            return (userId, authorizationHeader);
        }

        private static HttpRequestMessage BuildExpectedHttpRequestMessage(string url, string authorizationHeader)
        {
            return new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
                {
                    { HeaderNames.Authorization, authorizationHeader }
                }
            };
        }

        private static HttpResponseMessage BuildUserInfoEndpointResponse(string userId)
        {
            var userInfoContent = BuildUserInfoContent(userId);
            var serializedUserInfoContent = JsonConvert.SerializeObject(userInfoContent);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serializedUserInfoContent)
            };
        }

        private static object BuildUserInfoContent(string userId)
        {
            return new
            {
                sub = userId
            };
        }
    }
}
