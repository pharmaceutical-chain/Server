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
    }
}
