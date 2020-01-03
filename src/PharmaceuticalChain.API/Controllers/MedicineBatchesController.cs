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
    public class MedicineBatchesController : ControllerBase
    {
        private readonly IMedicineBatchService medicineBatchService;
        private readonly IMedicineService medicineService;
        private readonly ITenantService tenantService;
        private readonly IUploadService uploadService;
        public MedicineBatchesController(
            IMedicineBatchService medicineBatchService,
            IMedicineService medicineService,
            ITenantService tenantService,
            IUploadService uploadService
            )
        {
            this.medicineBatchService = medicineBatchService;
            this.medicineService = medicineService;
            this.tenantService = tenantService;
            this.uploadService = uploadService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMedicineAsync(
            [FromBody] CreateMedicineBatchCommand command)
        {
            try
            {
                if (command.ExpiryDate <= command.ManufactureDate)
                {
                    return BadRequest("Expiry date could not be less than or equal to Manufacture date.");
                }
                if (command.Quantity <= 0)
                {
                    return BadRequest("Cannot create a medicine batch less than 0 in quantity.");
                }
                if (medicineService.GetMedicine(command.MedicineId) == null)
                {
                    return BadRequest("MedicineId is not valid.");
                }
                if (tenantService.GetTenant(command.ManufacturerId) == null)
                {
                    return BadRequest("TenantId is not valid.");
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

                var result = await medicineBatchService.Create(
                    command.BatchNumber,
                    command.MedicineId,
                    command.ManufacturerId,
                    command.ManufactureDate,
                    command.ExpiryDate,
                    command.Quantity,
                    command.Unit,
                    command.Certificates);
                return Ok(new { MedicineBatchId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        public IActionResult GetMedicinesAsync()
        {
            try
            {
                var result = medicineBatchService.GetAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMedicineBatch(
            Guid id,
            [FromBody] CreateMedicineBatchCommand command)
        {
            try
            {
                await medicineBatchService.Update(
                    id,
                    command.BatchNumber,
                    command.MedicineId,
                    command.ManufactureDate,
                    command.ExpiryDate,
                    command.Quantity,
                    command.Unit,
                    command.Certificates,
                    command.IsApprovedByAdmin);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Delete a medicine batch from the blockchain and database.
        /// </summary>
        /// <param name="id">Id of the batch you want to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTenantAsync(Guid id)
        {
            try
            {
                await medicineBatchService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}