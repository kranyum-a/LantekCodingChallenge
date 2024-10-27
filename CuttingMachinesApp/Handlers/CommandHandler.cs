using CuttingMachinesApp.Interfaces;
using CuttingMachinesApp.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingMachinesApp.Handlers
{
   
    public class CommandHandler : ICommandHandler
    {
        private readonly ICuttingMachineService _machineService;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(ICuttingMachineService machineService, ILogger<CommandHandler> logger)
        {
            _machineService = machineService;
            _logger = logger;
        }
        public async Task HandleCommand(string command)
        {
            switch (command.ToLower())
            {
                case "help":
                    DisplayHelp();
                    break;
                case "all":
                    await DisplayMachines(null);
                    break;
                case "2d":
                    await DisplayMachines(2);
                    break;
                case "3d":
                    await DisplayMachines(3);
                    break;
                default:
                    _logger.LogWarning("Unknown command entered: {Command}", command);
                    Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    break;
            }
        }
        // Métodos privados DisplayHelp y DisplayMachines siguen iguales
        private void DisplayHelp()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("  help     - Show available commands.");
            Console.WriteLine("  all      - Show all cutting machines.");
            Console.WriteLine("  2d       - Show cutting machines with 2D technology.");
            Console.WriteLine("  3d       - Show cutting machines with 3D technology.");
            Console.WriteLine("  exit     - Exit the application.\n");
        }

        private async Task DisplayMachines(int? technology)
        {
            try
            {
                var machines = technology == null
                    ? await _machineService.GetAllMachinesAsync()
                    : await _machineService.GetMachinesByTechnologyAsync(technology.Value);

                if (machines.Count == 0)
                {
                    Console.WriteLine("No machines found.");
                    return;
                }

                Console.WriteLine(technology == null
                    ? "\nAll Cutting Machines:"
                    : $"\nCutting Machines ({technology}D Technology):");

                foreach (var machine in machines)
                {
                    Console.WriteLine($"Name: {machine.Name}");
                    Console.WriteLine($"Manufacturer: {machine.Manufacturer}");
                    Console.WriteLine($"Technology: {(machine.Technology == 2 ? "2D" : machine.Technology == 3 ? "3D" : "Unknown")}\n");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching machines.");
                Console.WriteLine("An error occurred while fetching machines. Please try again.");
            }
        }
    }
}
