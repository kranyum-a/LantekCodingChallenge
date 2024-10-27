using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using CuttingMachinesApp.Model;
using CuttingMachinesApp.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using Microsoft.Extensions.Configuration;
using CuttingMachinesApp.Handlers;
using CuttingMachinesApp.Interfaces;

namespace CuttingMachinesApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Set up Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Configure services
            
            var services = new ServiceCollection();            
            services.AddSingleton<IConfiguration>(configuration)
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog();
                    })
                    .AddHttpClient<CuttingMachineService>() 
                    .Services 
                    .AddSingleton<ICuttingMachineService, CuttingMachineService>() 
                    .AddTransient<ICommandHandler, CommandHandler>(); 

            var serviceProvider = services.BuildServiceProvider();

            var commandHandler = serviceProvider.GetRequiredService<ICommandHandler>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting Cutting Machines Console App");
            Console.WriteLine("Welcome to the Cutting Machines App!");
            Console.WriteLine("Type 'help' for a list of available commands.");

            while (true)
            {
                Console.Write("\nEnter command: ");
                var command = Console.ReadLine()?.ToLower();

                if (command == "exit")
                {
                    logger.LogInformation("Exiting application.");
                    break;
                }
                if (command != null)
                    await commandHandler.HandleCommand(command);
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("  help     - Show available commands.");
            Console.WriteLine("  all      - Show all cutting machines.");
            Console.WriteLine("  2d       - Show cutting machines with 2D technology.");
            Console.WriteLine("  3d       - Show cutting machines with 3D technology.");
            Console.WriteLine("  exit     - Exit the application.\n");
        }

        static async Task DisplayMachines(ICuttingMachineService machineService, Microsoft.Extensions.Logging.ILogger logger, int? technology)
        {
            try
            {
                List<CuttingMachine> machines;

                if (technology == null)
                {
                    machines = await machineService.GetAllMachinesAsync();
                    Console.WriteLine("\nAll Cutting Machines:");
                }
                else
                {
                    machines = await machineService.GetMachinesByTechnologyAsync(technology.Value);
                    Console.WriteLine($"\nCutting Machines ({technology}D Technology):");
                }

                foreach (var machine in machines)
                {
                    Console.WriteLine($"Name: {machine.Name}");
                    Console.WriteLine($"Manufacturer: {machine.Manufacturer}");
                    Console.WriteLine($"Technology: {(machine.Technology == 2 ? "2D" : machine.Technology == 3 ? "3D" : "Unknown")}\n");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching machines.");
                Console.WriteLine("An error occurred while fetching machines. Please try again.");
            }
        }
    }
}
