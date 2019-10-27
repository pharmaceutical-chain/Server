using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IMedicineBatchService
    {
        Task<Guid> Create(string CommercialName,
            string RegistrationCode,
            string BatchNumber,
            bool IsPrescriptionMedicine,
            string DosageForm,
            string IngredientConcentration,
            string PackingSpecification,
            uint Quantity,
            uint DeclaredPrice,
            DateTime ManufactureDate,
            DateTime ExpiryDate);
    }
}
