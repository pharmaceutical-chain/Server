using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        const String folderName = "files";
        readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        public UploadsController()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {
            using (var fileContentStream = new MemoryStream())
            {
                await myFile.CopyToAsync(fileContentStream);
                await System.IO.File.WriteAllBytesAsync(Path.Combine(folderPath, myFile.FileName), fileContentStream.ToArray());
            }
            return CreatedAtRoute(routeName: "myFile", routeValues: new { filename = myFile.FileName }, value: null); ;
        }

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
    }
} 