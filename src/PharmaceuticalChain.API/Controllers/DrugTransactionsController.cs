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
    public class DrugTransactionsController : ControllerBase
    {
        private readonly IDrugTransactionService drugTransactionService;
        public DrugTransactionsController(IDrugTransactionService drugTransactionService)
        {
            this.drugTransactionService = drugTransactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactions([FromBody] DrugTransactionInformation drugTransaction)
        {
            try
            {
                var result = await drugTransactionService.Create(
                    drugTransaction.FromCompanyId,
                    drugTransaction.ToCompanyId,
                    drugTransaction.DrugName,
                    drugTransaction.PackageId,
                    drugTransaction.Amount);
                return Ok(new { CompanyId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var result = await drugTransactionService.GetInformationOfAllDrugTransactions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}