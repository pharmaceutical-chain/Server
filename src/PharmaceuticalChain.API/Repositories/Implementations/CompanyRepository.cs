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

        void ICompanyRepository.Create(Tenant tenant)
        {
            dbContext.Tenants.Add(tenant);
            dbContext.SaveChanges();
        }

        Guid ICompanyRepository.CreateAndReturnId(Tenant tenant)
        {
            (this as ICompanyRepository).Create(tenant);
            return tenant.Id;
        }

        Tenant ICompanyRepository.Get(Guid companyId)
        {
            var result = dbContext.Tenants.Where(c => c.Id == companyId).SingleOrDefault();
            return result;
        }

        List<Tenant> ICompanyRepository.GetCompanies()
        {
            throw new NotImplementedException();
        }
    }
}
