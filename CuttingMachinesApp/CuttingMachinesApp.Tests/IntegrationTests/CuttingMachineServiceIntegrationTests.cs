using System.Threading.Tasks;
using CuttingMachinesApp.Interfaces;
using CuttingMachinesApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;

namespace CuttingMachinesApp.Tests.IntegrationTests
{
    public class CuttingMachineServiceIntegrationTests
    {
        private readonly ICuttingMachineService _service;

        public CuttingMachineServiceIntegrationTests()
        {
            var services = new ServiceCollection();
                        
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration)
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog();
                    })
                    .AddHttpClient<ICuttingMachineService, CuttingMachineService>();

            var serviceProvider = services.BuildServiceProvider();
            _service = serviceProvider.GetRequiredService<ICuttingMachineService>();
        }

       
        [Fact]
        public async Task GetAllMachinesAsync_ReturnsMachines_FromRealApi()
        {
            // Act
            var machines = await _service.GetAllMachinesAsync();

            // Assert
            Assert.NotNull(machines);
            Assert.NotEmpty(machines); 
        }

       
        [Fact]
        public async Task GetMachinesByTechnologyAsync_ReturnsMachines_FromRealApi()
        {
            // Act
            var machines = await _service.GetMachinesByTechnologyAsync(2); 

            // Assert
            Assert.NotNull(machines);
            
        }
    }
}
