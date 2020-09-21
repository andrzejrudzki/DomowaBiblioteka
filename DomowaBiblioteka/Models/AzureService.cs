using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DomowaBiblioteka.Models
{
    public class AzureService : IAzureService
    {

        private readonly IOptions<BibliotekaConfiguration> _config;

        public AzureService(IOptions<BibliotekaConfiguration> config)
        {
            _config = config;
        }

        public Task<string> AddBlobItem(string blobBase64)
        {
            BibliotekaAzureStorageClient azureStorageClient = new BibliotekaAzureStorageClient(_config.Value.StorageConnectionString, _config.Value.StorageContainerName);
            return azureStorageClient.AddBlobItem(blobBase64);

        }
        public Task DeleteBlobItem(string blobUri)
        {
            BibliotekaAzureStorageClient azureStorageClient = new BibliotekaAzureStorageClient(_config.Value.StorageConnectionString, _config.Value.StorageContainerName);
            return azureStorageClient.DeleteBlobItem(blobUri);

        }


    }
}
