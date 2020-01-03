using PharmaceuticalChain.API.Controllers.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IMedicineBatchService
    {
        Task<Guid> Create(
            string batchNumber,
            Guid medicineId,
            Guid manufacturerId,
            DateTime manufactureDate,
            DateTime expiryDate,
            uint Quantity,
            string Unit,
            string certificates);

        List<MedicineBatchQueryData> GetAll();

        /// <summary>
        /// Update every fields but not ManufacturerId.
        /// Normally, if a manufacturer creates an unwanted batch, it may delete or change the information accordingly, but not "change ownership" to another manufacturer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="batchNumber"></param>
        /// <param name="medicineId"></param>
        /// <param name="manufactureDate"></param>
        /// <param name="expiryDate"></param>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <param name="certificates"></param>
        /// <returns></returns>
        Task Update(
            Guid id,
            string batchNumber,
            Guid medicineId,
            DateTime manufactureDate,
            DateTime expiryDate,
            uint quantity,
            string unit,
            string certificates,
            bool isApprovedByAdmin);

        Task Delete(Guid id);
    }       
}
