using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;


namespace AzFuncStorage_POC
{
    public class WeatherPollingFunction
    {
        readonly static string ApiKey = Utilities.GetEnvironmentVariable("ApiKey");
        readonly static string ApiBaseUrl = Utilities.GetEnvironmentVariable("ApiBaseUrl");
        readonly static string ApiVersion = Utilities.GetEnvironmentVariable("ApiVersion");

        private static StorageHelper storageHelper = new StorageHelper();

        public static string BuildUrlQuery(string query, string units)
        {
            return $"{ApiBaseUrl}/data/{ApiVersion}/weather?q={query}&appid={ApiKey}&units={units}";
        }

        [FunctionName("PollWeather")]
        public static async Task Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            storageHelper.InitializeStorageAccount();

            try
            {
                HttpClient Client = new HttpClient();
                var city = Utilities.GetEnvironmentVariable("WeatherQueryCity");
                var apiUrl = BuildUrlQuery(city, "metric");

                log.LogInformation($"{DateTimeOffset.Now} - Retrieving Weather Update");
                var result = await Client.GetAsync(apiUrl);
                var requestBody = await result.Content.ReadAsStringAsync();
                var weatherInfo = JsonSerializer.Deserialize<WeatherReportResponse>(requestBody);

                if (weatherInfo == null)
                    throw new NullReferenceException("Unable to deserialize response from Api.");

                if (weatherInfo.cod != 200)
                    throw new OpenWeatherException(weatherInfo.cod, weatherInfo.message);

                log.LogInformation($"{DateTimeOffset.Now} - Retrieved.");
                log.LogInformation($"{DateTimeOffset.Now} - Weather in {city} : {weatherInfo.weather[0].description}");
                log.LogInformation($"{DateTimeOffset.Now} - Storing report in blob storage..");
                storageHelper.CreateBlobFileAsync($"report-{DateTimeOffset.Now.ToFileTime()}", requestBody);

            }
            catch (OpenWeatherException e)
            {
                log.LogError(e, "OpenWeather Exception: {Message}", e.Message);
                throw;
            }
            catch (NullReferenceException e)
            {
                log.LogError(e, "Null Exception: {Message}", e.Message);
                throw;
            }
            catch (Exception e)
            {
                log.LogError(e, "Unexpected Exception: {Message}", e.Message);
                throw;
            }


        }
    }
}
