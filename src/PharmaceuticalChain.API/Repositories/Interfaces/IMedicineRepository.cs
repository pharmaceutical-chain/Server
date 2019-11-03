using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface IMedicineRepository
    {
        void Create(Medicine medicine);
        Guid CreateAndReturnId(Medicine medicine);

        void Update(Medicine medicine);

        List<Medicine> GetAll();
        Medicine Get(Guid id);

        void Delete(Guid id);
    }
}
