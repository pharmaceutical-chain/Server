using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaceuticalChain.API.Auth0.Services.Interfaces;
using PharmaceuticalChain.API.Controllers.Models.Commands;
using PharmaceuticalChain.API.Services.Interfaces;

namespace PharmaceuticalChain.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService medicineService;
        private readonly IAuth0Service auth0Service;
        private readonly IUploadService uploadService;
        public MedicinesController(
            IMedicineService medicineService, 
            IAuth0Service auth0Service,
            IUploadService uploadService
            )
        {
            this.medicineService = medicineService;
            this.auth0Service = auth0Service;
            this.uploadService = uploadService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMedicineAsync(
            [FromBody] CreateMedicineCommand command)
        {
            try
            {
                if (command.DeclaredPrice <= 0)
                {
                    return BadRequest("Declared Price cannot be less than 0.");
                }

                if (!string.IsNullOrEmpty(command.Certificates))
                {
                    // Ensure each certificate provided in the command
                    List<string> certificateList = command.Certificates.Split(',').ToList();
                    foreach (var cert in certificateList)
                    {
                        if (string.IsNullOrEmpty(uploadService.GetFileUri(cert)))
                        {
                            return BadRequest("At least one certificate name provided is not valid.");
                        }
                    }
                }

                //var test = User.Claims.FirstOrDefault();
                //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //var client = new ManagementApiClient(auth0Service.ObtainAccessToken(), new Uri("https://pharmachain.au.auth0.com/api/v2/"));


                var result = await medicineService.Create(
                    command.CommercialName,
                    command.RegistrationCode,
                    command.IsPrescriptionMedicine,
                    command.DosageForm,
                    command.IngredientConcentration,
                    command.PackingSpecification,
                    command.DeclaredPrice,
                    command.CurrentlyLoggedInTenant,
                    command.Certificates);

                return Ok(new { MedicineId = result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMedicineAsync(
            Guid id,
            [FromBody] CreateMedicineCommand command)
        {
            try
            {
                await medicineService.Update(
                    id,
                    command.CommercialName,
                    command.RegistrationCode,
                    command.IsPrescriptionMedicine,
                    command.IngredientConcentration,
                    command.PackingSpecification,
                    command.DosageForm,
                    command.DeclaredPrice,
                    command.CurrentlyLoggedInTenant,
                    command.Certificates);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Return all medicines in the network/database.
        /// Might or might not include data of the tenant which submitted the medicine basing on the query parameter.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMedicinesAsync([FromQuery]bool? includeSubmittedTenant = true)
        {
            try
            {
                var result = medicineService.GetMedicines();

                if (includeSubmittedTenant.HasValue)
                {
                    if (includeSubmittedTenant == false)
                    {
                        foreach(var item in result)
                        {
                            item.SubmittedTenant = null;
                        }
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Query a medicine information with its Id.
        /// </summary>
        /// <returns>Return information about a medicine on the network.</returns>
        [HttpGet("{id}")]
        public IActionResult GetMedicine(Guid id)
        {
            try
            {
                var medicine = medicineService.GetMedicine(id);
                return Ok(medicine);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMedicine(Guid id)
        {
            try
            {
                await medicineService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}