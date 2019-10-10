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

        void ICompanyRepository.Create(Tenant company)
        {
            dbContext.Companies.Add(company);
            dbContext.SaveChanges();
        }

        Tenant ICompanyRepository.Get(int companyId)
        {
            var result = dbContext.Companies.Where(c => c.Id == companyId).SingleOrDefault();
            return result;
        }

        List<Tenant> ICompanyRepository.GetCompanies()
        {
            throw new NotImplementedException();
        }
    }
}
