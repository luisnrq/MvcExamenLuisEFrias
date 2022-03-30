using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcExamenLuisEFrias.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;
        private string containerName;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
            this.containerName = "examen";
        }

        public async Task UploadBlobAsync(string blobName, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(this.containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
