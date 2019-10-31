using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IMedicineBatchRepository
    {
        void Create(MedicineBatch medicineBatch);
        Guid CreateAndReturnId(MedicineBatch medicineBatch);

        void Update(MedicineBatch medicineBatch);

        List<MedicineBatch> GetAll();
        MedicineBatch Get(Guid id);

        void Delete(Guid id);
    }
}
