using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IMedicineService medicineBatchService;
        public MedicineBatchesController(IMedicineService medicineBatchService)
        {
            this.medicineBatchService = medicineBatchService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicineBatchAsync(
            [FromBody] CreateMedicineBatchCommand command)
        {
            try
            {
                var result = await medicineBatchService.Create(
                    command.CommercialName,
                    command.RegistrationCode,
                    command.BatchNumber,
                    command.IsPrescriptionMedicine,
                    command.DosageForm,
                    command.IngredientConcentration,
                    command.PackingSpecification,
                    command.Quantity,
                    command.DeclaredPrice,
                    command.ManufactureDate,
                    command.ExpiryDate);
                return Ok(new { MedicineBatchId = result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}