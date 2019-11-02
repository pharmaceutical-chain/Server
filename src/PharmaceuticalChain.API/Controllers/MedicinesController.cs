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
        public MedicinesController(IMedicineService medicineService, IAuth0Service auth0Service)
        {
            this.medicineService = medicineService;
            this.auth0Service = auth0Service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMedicineAsync(
            [FromBody] CreateMedicineCommand command)
        {
            try
            {
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
                    command.CurrentlyLoggedInTenant);

                return Ok(new { MedicineId = result });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}