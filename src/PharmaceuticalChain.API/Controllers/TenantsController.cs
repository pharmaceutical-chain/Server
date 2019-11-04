using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Controllers.Models.Commands;
using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService tenantService;
        public TenantsController(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        /// <summary>
        /// Send a transaction to create a new tenant on the Ethereum network.
        /// </summary>
        /// <param name="command">
        ///     Command with options to create new tenant.
        ///     Phone number is optional.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        ///     The API creates a transaction to create a new tenant on the network and returns right away.
        ///     It does not wait for the transaction to be mined to the network.
        ///     There will be background jobs to check the status of this transaction.
        ///     Consider using query APIs to get the status of the tenant or the transaction.
        /// </remarks>
        [HttpPost]
        [Authorize("create:users")]
        //[Authorize("roles:admin")]
        public async Task<IActionResult> CreateTenantAsync(
            [FromBody] CreateTenantCommand command)
        {
            try
            {
                if (command.PhoneNumber == null)
                    command.PhoneNumber = String.Empty;

                Enum.TryParse<TenantTypes>(command.Type, true, out TenantTypes tenantType);

                var result = await tenantService.Create(
                    command.Name, 
                    command.Email,
                    command.PrimaryAddress, 
                    command.PhoneNumber, 
                    command.TaxCode, 
                    command.RegistrationCode, 
                    command.GoodPractices,
                    tenantType);

                return Ok(new { CompanyId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a tenant from the blockchain and database.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize("roles:admin")]
        public async Task<IActionResult> DeleteTenantAsync(
            [FromBody] Guid tenantId)
        {
            try
            {
                await tenantService.Remove(tenantId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Query and return information of all tenants on the network/database.
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public async Task<IActionResult> GetTenants()
        {
            try
            {
                var tenants = await tenantService.GetAllTenants();
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Query a tenant information with its Id.
        /// </summary>
        /// <returns>Return information about a tenant.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenant(Guid id)
        {
            try
            {
                TenantQueryData tenant = tenantService.GetTenant(id);
                return Ok(tenant);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get contract address on the blockchain network that preresents a tenant object.
        /// </summary>
        /// <param name="tenantId">Id of the tenant</param>
        /// <returns>Contract address for the tenant on the blockchain.</returns>
        [HttpGet]
        [Route("{tenantId}/contract-address")]
        public async Task<IActionResult> GetContractAddress(Guid tenantId)
        {
            try
            {
                var result = await tenantService.GetContractAddress(tenantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}