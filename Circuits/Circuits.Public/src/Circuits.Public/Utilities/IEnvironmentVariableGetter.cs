namespace Circuits.Public.Utilities
{
    public interface IEnvironmentVariableGetter
    {
        string Get(string name);
    }
}