using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyChainsController : ControllerBase
    {
        private readonly ISupplyChainService supplyChainService;
        public SupplyChainsController(
            ISupplyChainService supplyChainService)
        {
            this.supplyChainService = supplyChainService;
        }

        [HttpGet("{batchId}")]
        public IActionResult GetSupplyChain(Guid batchId, [FromQuery]bool? isDetailed)
        {
            try
            {
                if (isDetailed.HasValue && isDetailed.Value == true)
                {
                    DetailedBatchSupplyChainQueryData result = supplyChainService.GetDetailedBatchSupplyChain(batchId);
                    return Ok(result);
                }
                else
                {
                    BatchSupplyChainQueryData result = supplyChainService.GetSimpleBatchSupplyChain(batchId);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}