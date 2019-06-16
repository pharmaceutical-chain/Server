using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        void ICompanyRepository.Create(Company company)
        {
            dbContext.Companies.Add(company);
            dbContext.SaveChanges();
        }

        List<Company> ICompanyRepository.GetCompanies()
        {
            throw new NotImplementedException();
        }
    }
}
