using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Utilities
{
    public static class MedicineBatchExtensionMethods
    {
        public static MedicineBatchQueryData ToMedicineBatchQueryData(this MedicineBatch medicineBatch)
        {
            if (medicineBatch == null) return null;
            var result = new MedicineBatchQueryData()
            {
                BatchNumber = medicineBatch.BatchNumber,
                ExpiryDate = medicineBatch.ExpiryDate,
                Id = medicineBatch.Id,
                ManufactureDate = medicineBatch.ManufactureDate,
                Manufacturer = medicineBatch.Manufacturer.ToTenantQueryData(),
                Medicine = medicineBatch.Medicine.ToMedicineQueryData(),
                Quantity = medicineBatch.Quantity,
                Unit = medicineBatch.Unit,
                Certificates = medicineBatch.Certificates,
                ContractAddress = medicineBatch.ContractAddress,
                DateCreated = medicineBatch.DateCreated,
                TransactionHash = medicineBatch.TransactionHash,
                TransactionStatus = medicineBatch.TransactionStatus.ToString(),
                IsApprovedByAdmin = medicineBatch.IsApprovedByAdmin
            };
            return result;
        }
    }
}
