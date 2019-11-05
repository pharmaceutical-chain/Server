using Nethereum.ABI.FunctionEncoding.Attributes;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class TenantQueryData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PrimaryAddress { get; set; }
        public List<string> BranchAddresses { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string RegistrationCode { get; set; }
        public string GoodPractices { get; set; }
        public string Type { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string TransactionStatus { get; set; }
    }
}
