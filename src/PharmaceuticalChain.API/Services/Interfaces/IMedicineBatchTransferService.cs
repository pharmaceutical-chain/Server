using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IMedicineBatchTransferService
    {
        Task<Guid> Create(
            Guid medicineBatchId,
            Guid fromTenantId,
            Guid toTenantId,
            uint quantity
            );
    }
}
