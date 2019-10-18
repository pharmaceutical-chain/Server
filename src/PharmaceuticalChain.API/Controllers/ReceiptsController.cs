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
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService receiptService;
        public ReceiptsController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        //[HttpPost]
        //public IActionResult CreateReceipt([FromBody] CreateReceiptCommand command)
        //{
        //    try
        //    {
        //        Guid receiptId = receiptService.CreateAndReturnId(command);
        //        return Ok(receiptId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetReceipts([FromQuery] int? companyId)
        //{
        //    try
        //    {
        //        if (companyId.HasValue)
        //        {
        //            var result = await receiptService.GetReceipts(companyId.Value);
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            // TODO: Get all
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}