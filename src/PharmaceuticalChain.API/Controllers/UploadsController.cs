using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        const String folderName = "files";
        private readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        private readonly IConfiguration configuration;

        public UploadsController(IConfiguration configuration)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            this.configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {
            try
            {
                var uniqueFileName = myFile.FileName + Guid.NewGuid().ToString();

                // Upload the file to Blob Storage
                var fileContentStream = myFile.OpenReadStream();
                string uploadedUri = await UploadToBlob(uniqueFileName, fileContentStream);

                // Save the file URI to database for later queries.
                // UniqueName -- Uri


                return CreatedAtRoute(routeName: "myFile", routeValues: new { filename = myFile.FileName }, value: null);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Use this API to get the URI of the file on Azure Blob Storage system or the file in download dialogue.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet("{filename}", Name = "myFile")]
        public async Task<IActionResult> Get([FromRoute] String filename)
        {
            var filePath = Path.Combine(folderPath, filename);
            if (System.IO.File.Exists(filePath))
            {
                return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", filename);
            }
            return NotFound();
        }


        private async Task<string> UploadToBlob(string fileName, Stream stream = null)
        {
            string storageConnectionString = configuration["StorageConnectionString"];

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "tenant-" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            
            try
            {
            // Open the file and upload its data
            await blobClient.UploadAsync(stream);

            }
            catch(Exception ex)
            {
                throw ex;
            }

            return blobClient.Uri.AbsoluteUri;
        }
    }
} 