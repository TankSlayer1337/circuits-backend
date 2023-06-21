using Circuits.Public.Http;

namespace Circuits.Public.Tests.Mockers
{
    public class HttpClientWrapperMocker : Mocker<IHttpClientWrapper>
    {
        public void SimulateSendAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            Mock.Setup(mock => mock.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Callback<HttpRequestMessage>((httpRequest) =>
                {
                    httpRequest.Should().BeEquivalentTo(request);
                })
                .Returns(Task.FromResult(response));
        }
    }
}
