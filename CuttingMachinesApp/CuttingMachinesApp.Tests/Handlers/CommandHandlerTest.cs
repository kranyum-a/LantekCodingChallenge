using System.Threading.Tasks;
using CuttingMachinesApp.Handlers;
using CuttingMachinesApp.Interfaces;
using CuttingMachinesApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CuttingMachinesApp.Tests.Handlers
{
#pragma warning disable CS8602 // Deshabilita la advertencia de referencia nula
    public class CommandHandlerTests
    {
        [Fact]
        public async Task HandleCommand_DisplaysHelp_WhenCommandIsHelp()
        {
            // Arrange
            var machineServiceMock = new Mock<ICuttingMachineService>();
            var loggerMock = new Mock<ILogger<CommandHandler>>();
            ICommandHandler commandHandler = new CommandHandler(machineServiceMock.Object, loggerMock.Object);

            // Act
            await commandHandler.HandleCommand("help");

            // Assert
            machineServiceMock.Verify(m => m.GetAllMachinesAsync(), Times.Never);
            machineServiceMock.Verify(m => m.GetMachinesByTechnologyAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_CallsGetAllMachinesAsync_WhenCommandIsAll()
        {
            // Arrange
            var machineServiceMock = new Mock<ICuttingMachineService>();
            var loggerMock = new Mock<ILogger<CommandHandler>>();
            ICommandHandler commandHandler = new CommandHandler(machineServiceMock.Object, loggerMock.Object);

            // Act
            await commandHandler.HandleCommand("all");

            // Assert
            machineServiceMock.Verify(m => m.GetAllMachinesAsync(), Times.Once);
            machineServiceMock.Verify(m => m.GetMachinesByTechnologyAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_CallsGetMachinesByTechnologyAsync_WhenCommandIs2d()
        {
            // Arrange
            var machineServiceMock = new Mock<ICuttingMachineService>();
            var loggerMock = new Mock<ILogger<CommandHandler>>();
            ICommandHandler commandHandler = new CommandHandler(machineServiceMock.Object, loggerMock.Object);

            // Act
            await commandHandler.HandleCommand("2d");

            // Assert
            machineServiceMock.Verify(m => m.GetAllMachinesAsync(), Times.Never);
            machineServiceMock.Verify(m => m.GetMachinesByTechnologyAsync(2), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_CallsGetMachinesByTechnologyAsync_WhenCommandIs3d()
        {
            // Arrange
            var machineServiceMock = new Mock<ICuttingMachineService>();
            var loggerMock = new Mock<ILogger<CommandHandler>>();
            ICommandHandler commandHandler = new CommandHandler(machineServiceMock.Object, loggerMock.Object);

            // Act
            await commandHandler.HandleCommand("3d");

            // Assert
            machineServiceMock.Verify(m => m.GetAllMachinesAsync(), Times.Never);
            machineServiceMock.Verify(m => m.GetMachinesByTechnologyAsync(3), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_LogsWarning_WhenCommandIsUnknown()
        {
            // Arrange
            var machineServiceMock = new Mock<ICuttingMachineService>();
            var loggerMock = new Mock<ILogger<CommandHandler>>();
            ICommandHandler commandHandler = new CommandHandler(machineServiceMock.Object, loggerMock.Object);
            var unknownCommand = "invalid";

            // Act
            await commandHandler.HandleCommand(unknownCommand);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Unknown command entered: {unknownCommand}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
#pragma warning restore CS8602
}
