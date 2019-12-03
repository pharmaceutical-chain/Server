using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationsController : ControllerBase
    {
        private readonly IVerificationService verificationService;
        public VerificationsController(
            IVerificationService verificationService)
        {
            this.verificationService = verificationService;
        }

        /// <summary>
        /// Aimed for pharmacy retailer usages.
        /// This API accepts retailerId and batchIds (of which a customer buys) and returns a QR code (in base64 format) decoding to a Verificator link.
        /// Customer can use that Verificator link to verify drugs.
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="batchIds"></param>
        /// <returns>Return a QR code in base64 format. The QR code contains a link to Verificator with retailerId and batchIds provided.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetVerificatorLink(
            [FromQuery] Guid retailerId,
            [FromQuery] List<Guid> batchIds
            )
        {
            try
            {
                var rawVerificatorLink = verificationService.CreateVerificatorLink(retailerId, batchIds);
                var shortenedLink = await verificationService.CreateShortenedLink(rawVerificatorLink);
                var base64QRCode = verificationService.CreateQRCode(shortenedLink);
                return Ok(base64QRCode);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}