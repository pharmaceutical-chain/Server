using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Utilities
{
    public static class MedicineExtensionMethods
    {
        public static MedicineQueryData ToMedicineQueryData (this Medicine medicine)
        {
            if (medicine == null) return null;
            var result = new MedicineQueryData()
            {
                Id = medicine.Id,
                BatchNumber = medicine.BatchNumber,
                CommercialName = medicine.CommercialName,
                DeclaredPrice = medicine.DeclaredPrice,
                DosageForm = medicine.DosageForm,
                IngredientConcentration = medicine.IngredientConcentration,
                IsPrescriptionMedicine = medicine.IsPrescriptionMedicine,
                PackingSpecification = medicine.PackingSpecification,
                RegistrationCode = medicine.RegistrationCode,
                SubmittedTenant = medicine.SubmittedTenant.ToTenantQueryData(),

                ContractAddress = medicine.ContractAddress,
                DateCreated = medicine.DateCreated,
                TransactionHash = medicine.TransactionHash,
                TransactionStatus = medicine.TransactionStatus.ToString()
            };
            return result;
        }
    }
}
