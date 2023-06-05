namespace Circuits.Public.Tests.Mockers
{
    public abstract class Mocker<T> where T : class
    {
        public Mock<T> Mock = new Mock<T>(MockBehavior.Strict);
    }
}
