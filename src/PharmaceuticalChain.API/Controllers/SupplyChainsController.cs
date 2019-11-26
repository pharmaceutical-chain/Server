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

        /// <summary>
        /// Get supply chain for a medicine batch.
        /// This API supports 2 types of data format right now:
        ///     - The simple (Recommended): In theory, provide nodes (Tenants) and edges (Transfers) of the supply chain. Using these information, a client can then draw a supply chain.
        ///     - The detailed: (Still in progress) Split the supply chain into separated chains from manufacturer to the last tenant.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="isDetailed">
        ///     Specify `false` or don't pass this query to use the simple query. (RECOMMENDED)
        ///     Specify `true` to use a detailed query.
        /// </param>
        /// <returns></returns>
        [HttpGet("{batchId}")]
        public IActionResult GetSupplyChain(Guid batchId, 
            [FromQuery]bool? isDetailed)
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

        /// <summary>
        /// Use for end-users who buy pharmaceutical products to query products they have bought from an exact retailer.
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet("{retailerId}/{batchId}")]
        public IActionResult GetSupplyChainAtExactRetailer(
            Guid retailerId,
            Guid batchId)
        {
            try
            {
                var result = supplyChainService.GetBatchSupplyChain(batchId, retailerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}