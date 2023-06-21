namespace Circuits.Public.Utilities
{
    public class EnvironmentVariableGetter : IEnvironmentVariableGetter
    {
        public string Get(string name)
        {
            return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Missing environment variable {name}");
        }
    }
}
