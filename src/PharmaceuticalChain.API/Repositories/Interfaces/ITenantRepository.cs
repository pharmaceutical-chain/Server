using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface ITenantRepository
    {
        void Create(Tenant company);
        Guid CreateAndReturnId(Tenant tenant);

        void Update(Tenant tenant);

        List<Tenant> GetAll();
        Tenant Get(Guid companyId);

        void Delete(Guid id);
    }
}
