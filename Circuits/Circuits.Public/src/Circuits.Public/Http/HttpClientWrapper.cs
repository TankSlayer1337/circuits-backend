namespace Circuits.Public.Http
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            return await httpClient.SendAsync(request);
        }
    }
}
