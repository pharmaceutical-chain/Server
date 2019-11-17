using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Utilities
{
    public static class MedicineBatchTransferExtensionMethods
    {
        public static MedicineBatchTransferQueryData ToMedicineBatchTransferQueryData(this MedicineBatchTransfer transfer)
        {
            if (transfer == null) return null;
            var result = new MedicineBatchTransferQueryData()
            {
                From = transfer.From.ToTenantQueryData(),
                To = transfer.To.ToTenantQueryData(),
                Id = transfer.Id,
                MedicineBatch = transfer.MedicineBatch.ToMedicineBatchQueryData(),
                Quantity = transfer.Quantity,
                ContractAddress = transfer.ContractAddress,
                DateCreated = transfer.DateCreated,
                TransactionHash = transfer.TransactionHash,
                TransactionStatus = transfer.TransactionStatus.ToString()
            };
            return result;
        }
    }
}
