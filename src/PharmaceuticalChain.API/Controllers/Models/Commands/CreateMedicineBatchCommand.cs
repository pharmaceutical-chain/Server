using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Commands
{
    public class CreateMedicineBatchCommand
    {
        public string CommercialName { get; set; }
        public string RegistrationCode { get; set; }
        public string BatchNumber { get; set; }
        public bool IsPrescriptionMedicine { get; set; }
        public string DosageForm { get; set; }
        public string IngredientConcentration { get; set; }
        public string PackingSpecification { get; set; }

        public uint Quantity { get; set; }
        public uint DeclaredPrice { get; set; }

        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
