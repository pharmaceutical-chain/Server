using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Controllers.Models.Commands;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineBatchTransfersController : ControllerBase
    {
        private readonly IMedicineBatchTransferService medicineBatchTransferService;
        public MedicineBatchTransfersController(IMedicineBatchTransferService medicineBatchTransferService)
        {
            this.medicineBatchTransferService = medicineBatchTransferService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransferAsync(
            [FromBody] CreateMedicineBatchTransferCommand command
            )
        {
            try
            {
                var result = await medicineBatchTransferService.Create(
                    command.MedicineBatchId,
                    command.FromTenantId,
                    command.ToTenantId,
                    command.Quantity);
                return Ok(new { MedicineBatchTransferId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}