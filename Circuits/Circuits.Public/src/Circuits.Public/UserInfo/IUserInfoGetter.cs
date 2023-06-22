namespace Circuits.Public.UserInfo
{
    public interface IUserInfoGetter
    {
        Task<string> GetUserIdAsync(string authorizationHeader);
    }
}