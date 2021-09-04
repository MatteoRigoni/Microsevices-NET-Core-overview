using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace platformservice.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        public CommandDataClient(HttpClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = client;

        }
        public async Task SendPlatformToCommand(PlatformReadDto platformDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"{_configuration["CommandServiceUrl"]}/api/c/platforms", httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to command service is ok");
            }
            else
            {
                Console.WriteLine("--> Sync POST to command service is failed");
            }
        }
    }
}