using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class MedicineBatchQueryData
    {
        public Guid Id { get; set; }
        public string BatchNumber { get; set; }

        public MedicineQueryData Medicine { get; set; }

        public TenantQueryData Manufacturer { get; set; }

        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public uint Quantity { get; set; }
        public string Unit { get; set; }
        public string Certificates { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string TransactionStatus { get; set; }

        public bool IsApprovedByAdmin { get; set; }
    }
}
