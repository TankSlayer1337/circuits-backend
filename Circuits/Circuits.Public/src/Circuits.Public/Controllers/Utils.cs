namespace Circuits.Public.Controllers
{
    public static class Utils
    {
        public static string GetAuthorizationHeader(HttpRequest request)
        {
            return request.Headers["Authorization"].First();
        }
    }
}
