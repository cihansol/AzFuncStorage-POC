# AzFuncStorage-POC
A Proof Of Content demonstrating the use of Function Apps, HTTP Client, Blob Storage. 
It will query the [OpenWeatherMap](https://openweathermap.org/) service to retrieve the current weather for a given Country City.

## Setup Instructions

- Make sure you have the required Azure Functions Core Tools installed.
- Clone the repo, create a new `local.settings.json` inside the project `src/` directory and populate it with the following:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "BlobStorageConnectionString": "UseDevelopmentStorage=true",
    "BlobStorageAccountName": "devstoreaccount1",
    "BlobStorageContainerName": "weather-reports",
    "ApiBaseUrl": "https://api.openweathermap.org",
    "ApiKey": "YOUR-API-KEY-FOR-OPENWEATHERMAP",
    "ApiVersion": "2.5",
    "WeatherQueryCity": "Country, City"
  }
}
```

- Grab a v2.0 API Key from [OpenWeatherMap.org](https://openweathermap.org/) and populate the `ApiKey` field inside the `local.settings.json`
- Replace the `WeatherQueryCity` value with a City of your liking. Eg. **"Australia, Sydney"**
- Make sure the Blob Storage Account exists on your local machine using [Azure Storage Explorer](https://azure.microsoft.com/en-us/products/storage/storage-explorer/) to check.

## Running
- Run the Function App, it should trigger the function every 5seconds pulling data from the Weather API and storing it as `weather-report-{filetime}.json` blob inside the container.

