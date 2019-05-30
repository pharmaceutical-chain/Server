using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    [FunctionOutput]
    public class CompanyInformation
    {
        [Parameter("uint256", 1)]
        public uint CompanyId { get; set; }

        [Parameter("bytes32", 2)]
        public string Name { get; set; }
    }
}
