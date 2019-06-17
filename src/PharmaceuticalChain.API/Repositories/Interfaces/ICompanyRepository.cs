using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        void Create(Company company);
        List<Company> GetCompanies();
        Company Get(int companyId);
    }
}
