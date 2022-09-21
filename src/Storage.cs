using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzFuncStorage_POC
{
    
    public class StorageHelper
    {
        static CloudStorageAccount cloudStorageAccount;
        static CloudBlobContainer cloudBlobContainer;
        static string storageConnectionString;
        static string storageContainerName;

        /// <Summary>
        /// Initializes the StorageAccount configuration based on keys found in .env
        /// </Summary>
        internal void InitializeStorageAccount()
        {
            // Reading .env files
            storageConnectionString = Utilities.GetEnvironmentVariable("BlobStorageConnectionString");
            storageContainerName = Utilities.GetEnvironmentVariable("BlobStorageContainerName");
            
            if (CloudStorageAccount.TryParse(storageConnectionString, out cloudStorageAccount))
            {
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                cloudBlobContainer = cloudBlobClient.GetContainerReference(storageContainerName);
                // Async?
                var result = cloudBlobContainer.CreateIfNotExistsAsync().Result;
            }
        }

        /// <Summary>
        /// Store a file in blob storage
        /// </Summary>
        internal async void CreateBlobFileAsync(string blobName, string jsonFile)
        {
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            await cloudBlockBlob.UploadTextAsync(jsonFile);
        }

    }
}