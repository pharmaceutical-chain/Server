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
        [Route("create-receipt")]
        public IActionResult CreateReceipt()
        {
            try
            {
                Guid receiptId = drugTransactionService.CreateAndReturnReceipt();
                return Ok(receiptId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactions([FromBody] DrugTransactionInformation drugTransaction)
        {
            try
            {
                if (drugTransaction.ReceiptId == null || drugTransaction.ReceiptId == Guid.Empty ||
                    !drugTransactionService.DoesReceiptExist(drugTransaction.ReceiptId))
                {
                    return BadRequest("Receipt doesn't exist. Please create receipt first, and passing its id in your drug transaction");
                }

                var result = await drugTransactionService.Create(
                    drugTransaction.FromCompanyId,
                    drugTransaction.ToCompanyId,
                    drugTransaction.DrugName,
                    drugTransaction.PackageId,
                    drugTransaction.Amount,
                    drugTransaction.ReceiptId);
                return Ok(result);
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