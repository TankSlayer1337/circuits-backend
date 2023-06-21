using Circuits.Public.Http;
using Circuits.Public.Utilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Circuits.Public.UserInfo
{
    public class UserInfoGetter : IUserInfoGetter
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IEnvironmentVariableGetter _environmentVariableGetter;

        public UserInfoGetter(IHttpClientWrapper httpClientWrapper, IEnvironmentVariableGetter environmentVariableGetter)
        {

            _httpClientWrapper = httpClientWrapper;
            _environmentVariableGetter = environmentVariableGetter;
        }

        public async Task<string> GetUserIdAsync(string authorizationHeader)
        {
            var userInfoEndpointUrl = _environmentVariableGetter.Get("USERINFO_ENDPOINT_URL");
            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, userInfoEndpointUrl)
            {
                Headers =
                {
                    { HeaderNames.Authorization, authorizationHeader }
                }
            };
            var response = await _httpClientWrapper.SendAsync(userInfoRequest);
            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            var serializedUserInfo = await response.Content.ReadAsStringAsync();
            var userInfo = JObject.Parse(serializedUserInfo);
            return userInfo.Value<string>("sub") ?? throw new Exception("Missing sub in UserInfo response.");
        }
    }
}
