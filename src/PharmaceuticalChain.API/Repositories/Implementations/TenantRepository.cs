using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Repositories.Implementations
{
    public class TenantRepository : BaseRepository, ITenantRepository
    {
        public TenantRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        void ITenantRepository.Create(Tenant tenant)
        {
            dbContext.Tenants.Add(tenant);
            dbContext.SaveChanges();
        }

        Guid ITenantRepository.CreateAndReturnId(Tenant tenant)
        {
            (this as ITenantRepository).Create(tenant);
            return tenant.Id;
        }

        void ITenantRepository.Delete(Guid id)
        {
            var tenantToBeDeleted = (this as ITenantRepository).Get(id);
            dbContext.Tenants.Remove(tenantToBeDeleted);
            dbContext.SaveChanges();
        }

        Tenant ITenantRepository.Get(Guid companyId)
        {
            var result = dbContext.Tenants.Where(c => c.Id == companyId).SingleOrDefault();
            return result;
        }

        List<Tenant> ITenantRepository.GetAll()
        {
            return dbContext.Tenants.ToList();
        }

        void ITenantRepository.Update(Tenant tenant)
        {
            dbContext.Tenants.Update(tenant);
            dbContext.SaveChanges();
        }
    }
}
