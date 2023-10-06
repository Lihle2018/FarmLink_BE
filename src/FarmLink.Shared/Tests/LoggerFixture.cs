using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;

namespace FarmLink.Shared.Tests
{
    public class LoggerFixture<T>
    {
        private readonly Mock<ILogger<T>> _loggerMock;
        private readonly IFixture _fixture;

        public LoggerFixture()
        {
            _loggerMock = new Mock<ILogger<T>>();
            _loggerMock.SetupAllProperties();
            _fixture = new Fixture();

            _fixture.Register(() => _loggerMock.Object);
        }

        public ILogger<T> GetRegisteredLogger()
        {
            return _loggerMock.Object;
        }
    }
}
