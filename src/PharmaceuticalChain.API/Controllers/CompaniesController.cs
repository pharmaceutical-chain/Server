using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ICompanyService companyService;
        public TenantsController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        /// <summary>
        /// Create a new company on the Ethereum network.
        /// </summary>
        /// <param name="name">Name of the company</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCompanyAsync(
            [FromBody] CreateCompanyCommand command)
        {
            try
            {
                var result = await companyService.Create(
                    command.Name, 
                    command.Address, 
                    command.PhoneNumber, 
                    command.TaxCode, 
                    command.BRCLink, 
                    command.GPCLink);
                return Ok(new { CompanyId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpGet]
        //[Route("total")]
        //public async Task<IActionResult> GetTotalAsync()
        //{
        //    try
        //    {
        //        var result = await companyService.GetTotalCompanies();
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        ///// <summary>
        ///// Query and return information of companies on the Ethereum network.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetCompanies()
        //{
        //    try
        //    {
        //        var companies = await companyService.GetInformationOfAllCompanies();
        //        return Ok(companies);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpGet]
        //[Route("{companyId}/storage")]
        //public async Task<IActionResult> GetStorage(uint companyId)
        //{
        //    try
        //    {
        //        var result = await companyService.GetStorageInformation(companyId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        [HttpGet]
        [Route("{tenantId}/contract-address")]
        public async Task<IActionResult> GetContractAddress(Guid tenantId)
        {
            try
            {
                var result = await companyService.GetContractAddress(tenantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}