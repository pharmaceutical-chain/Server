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
            string Unit);

        List<MedicineBatchQueryData> GetAll();


        
    }       
}
