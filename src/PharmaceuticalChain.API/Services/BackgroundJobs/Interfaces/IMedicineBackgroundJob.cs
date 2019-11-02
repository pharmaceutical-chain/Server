using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces
{
    public interface IMedicineBackgroundJob
    {
        void WaitForTransactionToSuccessThenFinishCreatingMedicine(Medicine medicine);
    }
}
