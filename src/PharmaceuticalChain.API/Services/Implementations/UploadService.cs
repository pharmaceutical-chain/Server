using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class UploadService : IUploadService
    {
        private readonly IConfiguration configuration;
        private readonly string storageConnectionString;
        private readonly IResourceRepository resourceRepository;
        public UploadService(
            IConfiguration configuration,
            IResourceRepository resourceRepository)
        {
            this.configuration = configuration;
            this.resourceRepository = resourceRepository;

            storageConnectionString = this.configuration["StorageConnectionString"];
        }

        string IUploadService.SaveAzureBlobInfoToDatabaseAndReturnKey(string blobName, string uri)
        {
            var blobResource = new Resource()
            {
                Name = blobName,
                Uri = uri
            };
            var key = resourceRepository.CreateAndReturnKey(blobResource);
            return key;
        }

        /// <summary>
        /// Upload given file to Azure Blob Storage and return its unique name on Blob and the URI where the resource lives.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resourceType"></param>
        /// <returns>
        ///     First return value is the unique file name of the file on Azure Blob Storage.
        ///     Second return value is the URI of the resource on Azure Blob Storage.
        /// </returns>
        async Task<(string, string)> IUploadService.UploadFileToAzureBlob(IFormFile file, ResourceTypes resourceType)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "bug-container";
            switch (resourceType)
            {
                case (ResourceTypes.MedicineCertificates):
                    {
                        containerName = "medicine-certificates";
                        break;
                    }
                case (ResourceTypes.MedicineBatchCertificates):
                    {
                        containerName = "medicine-batch-certificates";
                        break;
                    }
                case (ResourceTypes.TenantCertificates):
                    {
                        containerName = "tenant-certificates";
                        break;
                    }
                case (ResourceTypes.Other):
                default:
                    {
                        containerName = "other";
                        break;
                    }
            }
            

            // Create the container and return a container client object
            var response = await blobServiceClient.GetBlobContainerClient(containerName)
                .CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            var fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            var uniqueFileName = fileName + "-" + Guid.NewGuid().ToString() + fileExtension;
            file.Headers["CONTENT-TYPE"] = this.GetFileContentType(file.FileName);
            var fileContentStream = file.OpenReadStream();
            BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);
            try
            {
                // Open the file and upload its data
                await blobClient.UploadAsync(fileContentStream);
                blobClient.SetHttpHeaders(
                    new Azure.Storage.Blobs.Models.BlobHttpHeaders() {
                        ContentType = file.ContentType
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (uniqueFileName, blobClient.Uri.AbsoluteUri);
        }

        public string GetFileContentType(string FilePath)
        {
            string ContentType = string.Empty;
            string Extension = System.IO.Path.GetExtension(FilePath).ToLower();

            switch (Extension)
            {
                case "pdf":
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                case "bmp":
                case ".bmp":
                    ContentType = "image/bmp";
                    break;
                case "gif":
                case ".gif":
                    ContentType = "image/gif";
                    break;
                case "png":
                case ".png":
                    ContentType = "image/png";
                    break;
                case "jpg":
                case ".jpg":
                    ContentType = "image/jpeg";
                    break;
                case "jpeg":
                case ".jpeg":
                    ContentType = "image/jpeg";
                    break;
                case "zip":
                case ".zip":
                    ContentType = "application/zip";
                    break;
                default:
                    ContentType = "application/octet-stream";
                    break;

            }
            return ContentType;
        }

        string IUploadService.GetFileUri(string fileName)
        {
            var resource = resourceRepository.Get(fileName);

            if (resource == null)
            {
                return null;
            }

            var resourceUri = resource.Uri;
            return resourceUri;
        }
    }
}
