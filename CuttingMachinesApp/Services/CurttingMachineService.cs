using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CuttingMachinesApp.Model;
using System.Collections.Generic;
using System.Net.Http.Json;
using CuttingMachinesApp.Interfaces;

namespace CuttingMachinesApp.Services
{
    
    public class CuttingMachineService : ICuttingMachineService
    {
        private readonly HttpClient _client;
        private ILogger<CuttingMachineService> _logger;
        private readonly string _username;
        private readonly string _password;
        private readonly string _apiUrl;

        public CuttingMachineService(HttpClient client, ILogger<CuttingMachineService> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _apiUrl = configuration["ApiSettings:Url"] ?? string.Empty;
            _username = configuration["ApiSettings:Username"] ?? string.Empty;
            _password = configuration["ApiSettings:Password"] ?? string.Empty;

            if (_client.DefaultRequestHeaders.Authorization == null)
            {
                var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }
        }
        /// <summary>
        /// Retrieves all cutting machines from the API.
        /// </summary>
        /// <returns>A list of cutting machines, or an empty list if an error occurs.</returns>
        public async Task<List<CuttingMachine>> GetAllMachinesAsync()
        {
            return await MakeApiRequestAsync(_apiUrl);
        }
        /// <summary>
        /// Retrieves cutting machines by the specified technology (2D or 3D) from the API.
        /// </summary>
        /// <param name="technology">The technology type (2 for 2D, 3 for 3D).</param>
        /// <returns>A list of cutting machines for the specified technology, or an empty list if an error occurs.</returns>
        public async Task<List<CuttingMachine>> GetMachinesByTechnologyAsync(int technology)
        {
            return await MakeApiRequestAsync($"{_apiUrl}/{technology}");
        }

        private async Task<List<CuttingMachine>> MakeApiRequestAsync(string url)
        {
            try
            {
                _logger.LogInformation("Sending request to {Url}", url);

                var response = await _client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Received non-success status code {StatusCode}", response.StatusCode);                    
                    return new List<CuttingMachine>();
                }

                var machines = await response.Content.ReadFromJsonAsync<List<CuttingMachine>>();
                _logger.LogInformation("Successfully retrieved {Count} machines from API", machines?.Count ?? 0);
                return machines ?? new List<CuttingMachine>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while making API request to {Url}", url);                
                return new List<CuttingMachine>();
            }
        }
    }
}
