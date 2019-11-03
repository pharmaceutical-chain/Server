using PharmaceuticalChain.API.Controllers.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<Guid> Create(
            string commercialName,
            string registrationCode,
            bool isPrescriptionMedicine,
            string dosageForm,
            string ingredientConcentration,
            string packingSpecification,
            uint declaredPrice,
            Guid submittedTenantId);

        Task Update(
            Guid medicineId,
            string commercialName,
            string registrationCode,
            bool isPrescriptionMedicine,
            string ingredientConcentration,
            string packingSpecification,
            string dosageForm,
            uint declaredPrice,
            Guid submittedTenantId
            );

        List<MedicineQueryData> GetMedicines();

        MedicineQueryData GetMedicine(Guid id);
    }
}
