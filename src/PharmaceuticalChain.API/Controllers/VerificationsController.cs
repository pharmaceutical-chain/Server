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

        [HttpGet()]
        public async Task<IActionResult> GetShortenedLink()
        {
            try
            {
                var link = await verificationService.CreateShortenedLink("https://www.thefreedictionary.com/verifications");
                return Ok(link);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}