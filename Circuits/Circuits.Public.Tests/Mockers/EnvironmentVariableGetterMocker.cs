using Circuits.Public.Utilities;

namespace Circuits.Public.Tests.Mockers
{
    public class EnvironmentVariableGetterMocker : Mocker<IEnvironmentVariableGetter>
    {
        public void SimulateGet(string name, string value)
        {
            Mock.Setup(mock => mock.Get(name)).Returns(value);
        }
    }
}
