using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Models.Ethereum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IDrugTransactionService
    {
        /// <summary>
        /// Create a transaction indicating that a company has sent some pills to another company.
        /// Although counted individually, pills should be sent in packages so <paramref name="packageId"/> is also required.
        /// </summary>
        /// <param name="fromCompany">Id of the orginial company.</param>
        /// <param name="toCompany">Id of the receiver company</param>
        /// <param name="pillName">Name of the drug.</param>
        /// <param name="packageId">Package Id which the drugs belong to.</param>
        /// <param name="value">Number of pills are being transfered with this transaction.</param>
        /// <param name="receiptId">A drug transaction belongs to a receipt.</param>
        Task<CreateDrugTransactionResult> Create(uint fromCompany, uint toCompany, string pillName, string packageId, uint value, Guid receiptId);

        Task<int> GetTotalTransactions();

        Task<List<DrugTransactionInformation>> GetInformationOfAllDrugTransactions();
        Guid CreateAndReturnReceipt();
        bool DoesReceiptExist(Guid receiptId);
    }
}
