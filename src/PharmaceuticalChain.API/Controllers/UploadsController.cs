using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        const String folderName = "files";
        private readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        private readonly IConfiguration configuration;
        private readonly IUploadService uploadService;

        public UploadsController(
            IConfiguration configuration,
            IUploadService uploadService)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            this.configuration = configuration;
            this.uploadService = uploadService;
        }

        [Authorize]
        [HttpPost("tenant-certificates")]
        public async Task<IActionResult> PostTenantCertificates(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadService.UploadFileToAzureBlob(myFile, ResourceTypes.TenantCertificates);
                var key = uploadService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [Authorize]
        [HttpPost("medicine-certificates")]
        public async Task<IActionResult> PostMedicineCertificates(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadService.UploadFileToAzureBlob(myFile, ResourceTypes.MedicineCertificates);
                var key = uploadService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [Authorize]
        [HttpPost("medicine-batch-certificates")]
        public async Task<IActionResult> PostMedicineBatchCertificates(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadService.UploadFileToAzureBlob(myFile, ResourceTypes.MedicineBatchCertificates);
                var key = uploadService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Use this API to get the URI of the file on Azure Blob Storage system or the file in download dialogue.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}", Name = "myFile")]
        public IActionResult Get([FromRoute] string fileName)
        {
            var uri = uploadService.GetFileUri(fileName);
            if (uri == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(uri);
            }
        }
    }
}