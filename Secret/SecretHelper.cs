using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Secret
{
    public static class SecretHelper
    {
        private static readonly IConfiguration configuration;

        private static readonly HttpClient client = new HttpClient();

        static SecretHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
        }

        public static async Task<string> GetSecret(string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{configuration["SecretHost"]}/Secret/{key}");

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var secretDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);
                if (secretDictionary != null && secretDictionary.TryGetValue("value", out var secret))
                {
                    return secret;
                }
                else
                {
                    throw new InvalidOperationException("The secret value could not be found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
