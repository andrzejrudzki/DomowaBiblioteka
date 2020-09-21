using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;


namespace DomowaBiblioteka.Models
{
    public class BibliotekaAzureStorageClient
    {

        private readonly string _storageConnectionString = "";
        private readonly string _storageContainerName = "";

        public BibliotekaAzureStorageClient(string storageConnectionString, string storageContainerName)
        {
            _storageConnectionString = storageConnectionString;
            _storageContainerName = storageContainerName;
        }

        public async Task<string> AddBlobItem(string base64EncodedString)
        {
            string blobName = System.Guid.NewGuid().ToString();
            var base64Stream = StreamExtensions.ConvertFromBase64(base64EncodedString);

            if (CloudStorageAccount.TryParse(_storageConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_storageContainerName);
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });


                var blob = container.GetBlockBlobReference(blobName);
                //await blob.UploadFromFileAsync((blobPath));
                await blob.UploadFromStreamAsync(base64Stream);
                return blob.Uri.ToString();
            }
            //}
            return "";
        }

        public async Task DeleteBlobItem(string blobUri)
        {
            if (CloudStorageAccount.TryParse(_storageConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_storageContainerName);
                var blockBlob = new CloudBlockBlob(new Uri(blobUri));
                var blobName = blockBlob.Name;
                var blob = container.GetBlobReference(blobName);

                await blob.DeleteIfExistsAsync();
            }
        }


    }
}
