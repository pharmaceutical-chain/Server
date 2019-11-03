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
        public MedicineBatchesController(IMedicineBatchService medicineBatchService)
        {
            this.medicineBatchService = medicineBatchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMedicineAsync(
            [FromBody] CreateMedicineBatchCommand command)
        {
            try
            {
                var result = await medicineBatchService.Create(
                    command.BatchNumber,
                    command.MedicineId,
                    command.ManufacturerId,
                    command.ManufactureDate,
                    command.ExpiryDate,
                    command.Quantity,
                    command.Unit);
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
    }
}