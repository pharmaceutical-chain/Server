using Microsoft.AspNetCore.Http;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IUploadService
    {
        Task<(string, string)> UploadFileToAzureBlob(IFormFile file, ResourceTypes resourceType = ResourceTypes.Other);

        string SaveAzureBlobInfoToDatabaseAndReturnKey(string blobName, string uri);

        string GetFileUri(string fileName);
    }
}
