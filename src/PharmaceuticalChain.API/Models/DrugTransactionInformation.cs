using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    [FunctionOutput]
    public class DrugTransactionInformation
    {
        [Parameter("uint256", 1)]
        public uint FromCompanyId { get; set; }

        [Parameter("uint256", 2)]
        public uint ToCompanyId { get; set; }

        [Parameter("bytes32", 3)]
        public string DrugName { get; set; }

        [Parameter("uint256", 4)]
        public uint Amount { get; set; }

        [Parameter("bytes32", 5)]
        public string PackageId { get; set; }

        [Parameter("uint256", 6)]
        public uint ManufactureDateInUnix { get; set; }

        [Parameter("uint256", 7)]
        public uint ExpirationDateInUnix { get; set; }

        internal bool HasParent(
            List<DrugTransactionInformation> transactionPool,
            Func<DrugTransactionInformation, bool> parentMatchCondition)
        {
            if (transactionPool.Any(parentMatchCondition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal DrugTransactionInformation Parent(
            List<DrugTransactionInformation> transactionPool,
            Func<DrugTransactionInformation, bool> parentMatchCondition)
        {
            return transactionPool.Single(parentMatchCondition);
        }

        public Guid ReceiptId { get; set; }

        public DateTime ManufactureDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
