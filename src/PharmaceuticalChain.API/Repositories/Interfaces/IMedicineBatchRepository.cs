using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IMedicineBatchRepository
    {
        void Create(Medicine medicineBatch);
        Guid CreateAndReturnId(Medicine medicineBatch);

        void Update(Medicine medicineBatch);

        List<Medicine> GetAll();
        Medicine Get(Guid id);

        void Delete(Guid id);
    }
}
