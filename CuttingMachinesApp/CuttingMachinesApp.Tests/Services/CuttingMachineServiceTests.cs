using System.Net;
using CuttingMachinesApp.Model;
using CuttingMachinesApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace CuttingMachinesApp.Tests.Services
{
#pragma warning disable CS8602 // Deshabilita la advertencia de referencia nula
    public class CuttingMachineServiceTests
    {
        private readonly Mock<IConfiguration> configurationMock;
        private readonly string _apiUrl = "https://app-academy-neu-codechallenge.azurewebsites.net/api/cut";
        public CuttingMachineServiceTests()
        {
            // Configuración simulada

            configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(config => config["ApiSettings:Url"]).Returns(_apiUrl);
        }
        [Fact]
        public async Task GetAllMachinesAsync_ReturnsMachines_WhenApiReturnsSuccess()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            var mockResponse = new List<CuttingMachine>
            {
                new CuttingMachine { Id = "1", Name = "GF3015", Manufacturer = "HGTech", Technology = 2 },
                new CuttingMachine { Id = "2", Name = "LT8.10", Manufacturer = "BLM Group", Technology = 2 }
            };

            mockHttp.When($"{_apiUrl}")
                    .Respond("application/json", System.Text.Json.JsonSerializer.Serialize(mockResponse));

            var httpClient = mockHttp.ToHttpClient();
            var loggerMock = new Mock<ILogger<CuttingMachineService>>();

            var service = new CuttingMachineService(httpClient, loggerMock.Object, configurationMock.Object);

            // Act
            var result = await service.GetAllMachinesAsync();
            var resultCount = result?.Count ?? 0;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, resultCount);
            Assert.Equal("GF3015", result[0].Name);
            Assert.Equal("BLM Group", result[1].Manufacturer);
        }

        [Fact]
        public async Task GetMachinesByTechnologyAsync_ReturnsMachines_WhenApiReturnsSuccess()
        {
            // Arrange
            int technology = 3;
            var mockHttp = new MockHttpMessageHandler();
            var mockResponse = new List<CuttingMachine>
            {
                new CuttingMachine { Id = "3", Name = "XYZ123", Manufacturer = "TechCorp", Technology = 3 }
            };

            mockHttp.When($"{_apiUrl}/{technology}")
                    .Respond("application/json", System.Text.Json.JsonSerializer.Serialize(mockResponse));

            var httpClient = mockHttp.ToHttpClient();
            var loggerMock = new Mock<ILogger<CuttingMachineService>>();
            var service = new CuttingMachineService(httpClient, loggerMock.Object, configurationMock.Object);

            // Act
            var result = await service.GetMachinesByTechnologyAsync(technology);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("XYZ123", result[0].Name);
            Assert.Equal("TechCorp", result[0].Manufacturer);
        }

        [Fact]
        public async Task GetAllMachinesAsync_LogsError_WhenApiReturnsFailure()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{_apiUrl}")
                    .Respond(HttpStatusCode.Unauthorized);

            var httpClient = mockHttp.ToHttpClient();
            var loggerMock = new Mock<ILogger<CuttingMachineService>>();
            var service = new CuttingMachineService(httpClient, loggerMock.Object, configurationMock.Object);

            // Act
            var result = await service.GetAllMachinesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            loggerMock.Verify(
             x => x.Log(
                 LogLevel.Warning,
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Received non-success status code")),
                 It.IsAny<Exception?>(),
                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
             Times.Once);
        }

        [Fact]
        public async Task GetMachinesByTechnologyAsync_HandlesExceptionGracefully()
        {
            // Arrange
            int technology = 2;
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{_apiUrl}/{technology}")
                    .Throw(new HttpRequestException("Network error"));

            var httpClient = mockHttp.ToHttpClient();
            var loggerMock = new Mock<ILogger<CuttingMachineService>>();
            var service = new CuttingMachineService(httpClient, loggerMock.Object, configurationMock.Object);

            // Act
            var result = await service.GetMachinesByTechnologyAsync(technology);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while making API request")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
#pragma warning restore CS8602
}