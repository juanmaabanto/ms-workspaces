using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.WebClients
{
    public class LoggingWebClient : ILoggingWebClient
    {
        private IOptions<WorkingSpaceSetting> _settings;

        public LoggingWebClient(IOptions<WorkingSpaceSetting> settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> ErrorAsync(string message, string trace, string username)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "dXNybG9nZ2luZzpBMTIzNDU2YQ==");

                    var dto = new {
                        source = Assembly.GetExecutingAssembly().GetName().Name, 
                        message, 
                        trace, 
                        username
                    };
                    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{_settings.Value.Services.LoggingUrl}/api/v1/events/errors", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("LoggingAPI status: " + response.StatusCode);
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en el servicio de logging: " + ex.Message);
                
                return string.Empty;
            }
        }
    }
}