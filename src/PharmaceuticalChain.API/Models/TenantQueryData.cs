using Nethereum.ABI.FunctionEncoding.Attributes;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class TenantQueryData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PrimaryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string BRCLink { get; set; }
        public string GPCLink { get; set; }
        public string TransactionHash { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
