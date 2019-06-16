using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IReceiptRepository
    {
        Guid CreateAndReturnId(Receipt receipt);


        /// <summary>
        /// Get receipts of a company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<Receipt> GetReceipts(uint companyId);

        Receipt GetReceipt(Guid receiptId);
    }
}
