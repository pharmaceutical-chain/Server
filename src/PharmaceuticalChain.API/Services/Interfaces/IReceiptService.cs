using PharmaceuticalChain.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IReceiptService
    {
        Task<ReceiptQuery> GetReceipts(int companyId);

        Guid CreateAndReturnId(CreateReceiptCommand command);
    }
}
