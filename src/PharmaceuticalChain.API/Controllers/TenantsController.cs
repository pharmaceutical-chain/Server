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
        private readonly IUploadService uploadService;
        public TenantsController(
            ITenantService tenantService,
            IUploadService uploadService)
        {
            this.tenantService = tenantService;
            this.uploadService = uploadService;
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
                if (String.IsNullOrEmpty(command.Email))
                {
                    return BadRequest("Email cannot be null or empty.");
                }
                if (!string.IsNullOrEmpty(command.Certificates))
                {
                    // Ensure each certificate provided in the command
                    List<string> certificateList = command.Certificates.Split(',').ToList();
                    foreach(var cert in certificateList)
                    {
                        if (string.IsNullOrEmpty(uploadService.GetFileUri(cert)))
                        {
                            return BadRequest("At least one certificate name provided is not valid.");
                        }
                    }
                }

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
                    command.Certificates,
                    tenantType);

                return Ok(new { CompanyId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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
        /// Update a tenant that has already existed on the blockchain and the database.
        /// </summary>
        /// <param name="id">Id of the tenant.</param>
        /// <param name="command">Information</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTenantAsync(
            Guid id,
            [FromBody] CreateTenantCommand command)
        {
            try
            {
                Enum.TryParse<TenantTypes>(command.Type, true, out TenantTypes tenantType);
                await tenantService.Update(
                    id,
                    command.Name,
                    command.Email,
                    command.PrimaryAddress,
                    command.PhoneNumber,
                    command.TaxCode,
                    command.RegistrationCode,
                    command.Certificates,
                    tenantType);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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