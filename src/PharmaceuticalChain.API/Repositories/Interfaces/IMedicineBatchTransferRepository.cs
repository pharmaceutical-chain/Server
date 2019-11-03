using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IMedicineBatchTransferRepository
    {
        void Create(MedicineBatchTransfer medicineBatchTransfer);
        Guid CreateAndReturnId(MedicineBatchTransfer medicineBatchTransfer);

        void Update(MedicineBatchTransfer medicineBatchTransfer);

        List<MedicineBatchTransfer> GetAll();
        MedicineBatchTransfer Get(Guid id);

        void Delete(Guid id);
    }
}
