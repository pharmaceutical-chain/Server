using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces
{
    interface IMedicineBatchTransferBackgroundJob
    {
        void WaitForTransactionToSuccessThenFinishCreating(MedicineBatchTransfer transfer);
    }
}
