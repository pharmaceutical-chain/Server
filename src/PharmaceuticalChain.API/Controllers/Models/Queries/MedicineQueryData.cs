using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class MedicineQueryData
    {
        public Guid Id { get; set; }

        public string CommercialName { get; set; }
        public string RegistrationCode { get; set; }
        public bool IsPrescriptionMedicine { get; set; }
        public string DosageForm { get; set; }
        public string IngredientConcentration { get; set; }
        public string PackingSpecification { get; set; }
        public uint DeclaredPrice { get; set; }

        public TenantQueryData SubmittedTenant { get; set; }

        public string Certificates { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string TransactionStatus { get; set; }
    }
}
